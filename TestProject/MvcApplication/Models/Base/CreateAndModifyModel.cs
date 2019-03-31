using System;

namespace MvcApplication.Models
{
    public class CreateModel
    {
        private string createIdFiled = string.Empty;
        public string CreateIdField
        {
            get
            {
                return createIdFiled;
            }
            set
            {
                this.createIdFiled = value;
            }
        }
        private string createTimeField = string.Empty;
        public string CreateTimeField
        {
            get
            {
                return createTimeField;
            }
            set
            {
                this.createTimeField = value;
            }
        }
        public int CreateId { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class ModifyModel
    {
        private string modifyIdField = string.Empty;
        public string ModifyIdField
        {
            get
            {
                return modifyIdField;
            }
            set
            {
                this.modifyIdField = value;
            }
        }
        private string modifyTimeField = string.Empty;
        public string ModifyTimeField
        {
            get
            {
                return modifyTimeField;
            }
            set
            {
                this.modifyTimeField = value;
            }
        }
        public int ModifyId { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}