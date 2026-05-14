using ArabRiver.Repository.Models;
using ArabRiver.Service.DTOs.Admin;
using ArabRiver.Service.DTOs.Auth;
using ArabRiver.Service.DTOs.Catalog;
using ArabRiver.Service.DTOs.Contact;
using ArabRiver.Service.DTOs.Lead;
using ArabRiver.Service.DTOs.OutsideEgyptVisitor;
using ArabRiver.Service.DTOs.Partner;
using AutoMapper;

namespace ArabRiver.Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // =========================
            // Catalog
            // =========================

            CreateMap<Catalog, CatalogResponseDto>()
                .ForMember(
                    dest => dest.PartnerName,
                    opt => opt.MapFrom(
                        src => src.Partner != null
                            ? src.Partner.Name
                            : null));

            CreateMap<CreateCatalogDto, Catalog>();

            CreateMap<UpdateCatalogDto, Catalog>();


            // =========================
            // Partner
            // =========================

            CreateMap<Partner, PartnerResponseDto>();


            // =========================
            // Lead
            // =========================

            CreateMap<Lead, LeadResponseDto>();

            CreateMap<CreateLeadDto, Lead>();


            // =========================
            // Contact Message
            // =========================

            CreateMap<ContactMessage,
                ContactMessageResponseDto>();

            CreateMap<CreateContactMessageDto,
                ContactMessage>();


            // =========================
            // Admin
            // =========================

            CreateMap<Admin, AdminResponseDto>();


            // =========================
            // Auth
            // =========================

            CreateMap<Admin, LoginResponseDto>();

            // =========================
            // Outside Egypt Visitors
            // =========================

            CreateMap<OutsideEgyptVisitor,
                OutsideEgyptVisitorResponseDto>();
        }
    }
}
