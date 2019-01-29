
namespace Director
{
    class InkVar
    {
        public string VarName { get; private set; }
        public object CurrentValue { get; private set; }
        public object NewValue { get; set; }


        public InkVar(string name, object initVal)
        {
            VarName = name;
            CurrentValue = initVal;
        }

        public void SetValue(object o)
        {
            CurrentValue = o;
        }
    }
}
