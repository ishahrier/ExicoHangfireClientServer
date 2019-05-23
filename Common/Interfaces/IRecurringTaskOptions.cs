namespace Exico.HF.Common.Interfaces {
    public interface IRecurringTaskOptions : IBaseTaskOptions {

        void SetCronExpression(string expression) ;
        string GetCronExpression();
    }
}