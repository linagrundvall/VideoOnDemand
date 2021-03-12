using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Database.Contexts;
using VOD.Database.Interfaces;
using VOD.Database.Services;

namespace VOD.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<VODContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
            services.AddScoped<ICRUDService, CRUDService>();
            services.AddScoped<IDbReadService, DbReadService>();

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Video, VideoDTO>()
                    .ReverseMap();

                cfg.CreateMap<Instructor, InstructorDTO>()
                    .ForMember(dest => dest.InstructorName, src => src.MapFrom(s => s.Name))
                    .ForMember(dest => dest.InstructorDescription, src => src.MapFrom(s => s.Description))
                    .ForMember(dest => dest.InstructorAvatar, src => src.MapFrom(s => s.Thumbnail));                                
                cfg.CreateMap<InstructorDTO, Instructor>()
                    .ForMember(dest => dest.Name, src => src.MapFrom(s => s.InstructorName))
                    .ForMember(dest => dest.Description, src => src.MapFrom(s => s.InstructorDescription))
                    .ForMember(dest => dest.Thumbnail, src => src.MapFrom(s => s.InstructorAvatar));

                cfg.CreateMap<Download, DownloadDTO>()
                    .ForMember(dest => dest.DownloadUrl, src => src.MapFrom(s => s.Url))
                    .ForMember(dest => dest.DownloadTitle, src => src.MapFrom(s => s.Title));
                cfg.CreateMap<DownloadDTO, Download>()
                    .ForMember(dest => dest.Url, src => src.MapFrom(s => s.DownloadUrl))
                    .ForMember(dest => dest.Title, src => src.MapFrom(s => s.DownloadTitle));

                cfg.CreateMap<Course, CourseDTO>()
                    .ForMember(dest => dest.CourseId, src => src.MapFrom(s => s.Id))
                    .ForMember(dest => dest.CourseTitle, src => src.MapFrom(s => s.Title))
                    .ForMember(dest => dest.CourseDescription, src => src.MapFrom(s => s.Description))
                    .ForMember(dest => dest.MarqueeImageUrl, src => src.MapFrom(s => s.MarqueeImageUrl))
                    .ForMember(dest => dest.CourseImageUrl, src => src.MapFrom(s => s.ImageUrl))
                    .ForMember(dest => dest.Instructor, src => src.MapFrom(s => s.Instructor.Name));
                cfg.CreateMap<CourseDTO, Course>()
                    .ForMember(dest => dest.Id, src => src.MapFrom(s => s.CourseId))
                    .ForMember(dest => dest.Title, src => src.MapFrom(s => s.CourseTitle))
                    .ForMember(dest => dest.Description, src => src.MapFrom(s => s.CourseDescription))
                    .ForMember(dest => dest.ImageUrl, src => src.MapFrom(s => s.CourseImageUrl));
                    //.ForMember(dest => dest.Instructor, src => src.MapFrom(s => s.Instructor))

                cfg.CreateMap<Module, ModuleDTO>()
                    .ForMember(dest => dest.ModuleTitle, src => src.MapFrom(s => s.Title));
                cfg.CreateMap<ModuleDTO, Module>()
                    .ForMember(dest => dest.Title, src => src.MapFrom(s => s.ModuleTitle));
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
