using AutoMapper;
using ModelsLibrary.Models;
using ModelsLibrary.Models.DTO;

namespace ModelsLibrary.Utilities
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Lab, LabCreationDTO>().ReverseMap();
            CreateMap<Computer,ComputerDTO>().ReverseMap();
            CreateMap<Computer,ComputerCreationDTO>().ReverseMap();
            CreateMap<Issue,IssueDTO>().ReverseMap();  
            CreateMap<Issue,IssueCreationDTO>().ReverseMap();   
        }
    }
}
