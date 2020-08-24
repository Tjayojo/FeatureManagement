using AutoMapper;
using Microsoft.FeatureManagement.Core.DTO;

namespace Microsoft.FeatureManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class MappingProfile : Profile
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Feature, Data.Models.Feature>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<TimeWindow, Data.Models.TimeWindow>().ReverseMap();
            CreateMap<RolloutPercentage, Data.Models.RolloutPercentage>().ReverseMap();
            CreateMap<Audience, Data.Models.Audience>().ReverseMap();
            CreateMap<BrowserRestriction, Data.Models.BrowserRestriction>().ReverseMap();
            CreateMap<User, Data.Models.User>().ReverseMap();
            CreateMap<GroupRollout, Data.Models.GroupRollout>().ReverseMap();
            CreateMap<SupportedBrowser, Data.Models.SupportedBrowser>().ReverseMap();
        }

        #endregion
    }
}