using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common;
using VVRestApi.Objects.Enums;

namespace VVRestApi.Objects.Models
{
    public class ObjectModel : RestObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid ModelId { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid CreateById { get; set; }

        public string CreateBy { get; set; }

        public DateTime ModifyDate { get; set; }

        public Guid ModifyById { get; set; }

        public string ModifyBy { get; set; }

        public int Revision { get; set; }

        public Guid RevisionId { get; set; }

        public Dictionary<string, object> Properties { get; set; } = null;

        public IList<RelatedObjectModel> RelatedObjects { get; set; } = null;
    }

    public class RelatedObjectModel : ObjectModel
    {
        public RelatedModelRole ModelRole { get; set; }
    }
}
