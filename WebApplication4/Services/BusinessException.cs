namespace WebApplication4.Services
{
    /// <summary>
    /// Бизнес-исключение: сервис сообщает о нарушении бизнес-правила понятным текстом.
    /// Сервис не знает про HTTP; контроллер переводит это исключение в код 409/400,
    /// а не в 500.
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}
