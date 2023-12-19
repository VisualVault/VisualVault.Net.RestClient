using System;

namespace VVRestApi.Studio.Models
{
    public class WorkflowVariable
    {
        public WorkflowVariable()
        {
        }

        public WorkflowVariable(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public WorkflowVariable(string name, string value)
        {
            Name = name;
            Value = value;
            DataType = WorkflowVariableType.Text;
        }

        public WorkflowVariable(string name, decimal value)
        {
            Name = name;
            Value = value;
            DataType = WorkflowVariableType.Number;
        }
        public WorkflowVariable(string name, DateTime value)
        {
            Name = name;
            Value = value;
            DataType = WorkflowVariableType.Date;
        }

        public WorkflowVariable(string name, bool value)
        {
            Name = name;
            Value = value;
            DataType = WorkflowVariableType.Boolean;
        }

        public WorkflowVariable(string name, WorkflowVariable value)
        {
            Name = name;
            Value = value;
            DataType = WorkflowVariableType.DataSet;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public WorkflowVariableType DataType { get; set; }
    }
}
