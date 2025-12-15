using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Notifications;
using VVRestApi.Objects.Models;

namespace VVRestApi.Objects.Model
{
    public class ModelsManager : ObjectsApi
    {
        protected ModelsManager() { }

        public ModelsManager(ObjectsApi api)
        {
            base.Populate(api);
        }

        public IEnumerable<ModelModel> GetModels()
        {
            return HttpHelper.GetListResult<ModelModel>(GlobalConfiguration.RoutesObjectsApi.GetModels, string.Empty, null, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public ModelModel GetModel(Guid id)
        {
            return HttpHelper.Get<ModelModel>(GlobalConfiguration.RoutesObjectsApi.GetModel, string.Empty, null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, false, id);
        }
    }
}
