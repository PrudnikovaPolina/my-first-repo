namespace Laba3
{
    internal class Program
    {
        static void Main(string[] args)
        {

                //тест Singleton 
                Console.WriteLine("1. Проверка Singleton:");
                NotificationSettings settings1 = NotificationSettings.GetInstance();
                NotificationSettings settings2 = NotificationSettings.GetInstance();

                Console.WriteLine($"Имя отправителя в settings1: {settings1.SenderName}");             
                settings2.SenderName = "НИТУ МИСИС (Обновлено)"; //меняем данные в settings2
                Console.WriteLine($"Имя отправителя в settings1 после изменения через settings2: {settings1.SenderName}");
                Console.WriteLine($"Обе переменные ведут на один объект? {object.ReferenceEquals(settings1, settings2)}\n");


                //тест Abstract Factory + Builder 
                Console.WriteLine("2. Проверка Abstract Factory и Builder:");

                
                INotificationStyleFactory officialStyle = new OfficialStyleFactory();//задаем официальный стиль с помощью фабрики стилей
                IMessageBuilder builder = new StyledMessageBuilder(officialStyle);

                //строитель пошагово собирает структуру сообщения
                builder.BuildHeader();
                builder.BuildBody("Завтра состоится защита лабораторных работ по ООП.");
                builder.BuildFooter();

                FullMessage officialMessage = builder.GetResult();
                string readyText = officialMessage.GetFullText();

                Console.WriteLine("Строитель успешно собрал текст сообщения:");
                Console.WriteLine(readyText);
                Console.WriteLine();


                //тест Factory Method 
                Console.WriteLine("3. Проверка Factory Method:");

                //создаем фабрику для Email и отправляем текст
                NotifierFactory emailFactory = new EmailNotifierFactory();
                emailFactory.SendNotification(readyText);

                Console.WriteLine();

                //создаем фабрику для SMS и отправляем тот же текст
                NotifierFactory smsFactory = new SmsNotifierFactory();
                smsFactory.SendNotification(readyText);

                Console.ReadLine(); 
            

    }
}

    //Singleton
    public class NotificationSettings
    {
        private static NotificationSettings _instance;

        public string SenderName { get; set; }
        public int MaxTextLength { get; set; }

        private NotificationSettings()
        {
            SenderName = "МИСИС Университет";
            MaxTextLength = 160; 
        }

        public static NotificationSettings GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NotificationSettings();
            }
            return _instance;
        }
    }



    //Factory metod
    public interface INotifier
    {
        void Send(string text);
    }

    public class EmailNotifier : INotifier
    {
        public void Send(string text)
        {
            var settings = NotificationSettings.GetInstance(); 
            Console.WriteLine($"[Email от {settings.SenderName}]: {text}");
        }
    }

    public class SmsNotifier : INotifier
    {
        public void Send(string text)
        {
            var settings = NotificationSettings.GetInstance();
            Console.WriteLine($"[SMS от {settings.SenderName}]: {text}");
        }
    }
 
    public abstract class NotifierFactory
    {
        public abstract INotifier CreateNotifier();

        public void SendNotification(string text)
        {
            INotifier notifier = CreateNotifier(); 
            notifier.Send(text);
        }
    }

    public class EmailNotifierFactory : NotifierFactory
    {
        public override INotifier CreateNotifier() => new EmailNotifier();
    }

    public class SmsNotifierFactory : NotifierFactory
    {
        public override INotifier CreateNotifier() => new SmsNotifier();
    }



    //Abstract factory
    public interface IHeader { string GetText(); }
    public interface IFooter { string GetText(); }

    //семейство 1 - официальный стиль
    public class OfficialHeader : IHeader { public string GetText() => "УВАЖАЕМЫЙ СТУДЕНТ!"; }
    public interface IOfficialFooter : IFooter { } 
    public class OfficialFooter : IFooter { public string GetText() => "С уважением, Администрация."; }

    //семейство 2 - студенческий стиль 
    public class StudentHeader : IHeader { public string GetText() => "Привет, друг!"; }
    public class StudentFooter : IFooter { public string GetText() => "Увидимся на парах!"; }
    public interface INotificationStyleFactory
    {
        IHeader CreateHeader();
        IFooter CreateFooter();
    }

    public class OfficialStyleFactory : INotificationStyleFactory
    {
        public IHeader CreateHeader() => new OfficialHeader();
        public IFooter CreateFooter() => new OfficialFooter();
    }

    public class StudentStyleFactory : INotificationStyleFactory
    {
        public IHeader CreateHeader() => new StudentHeader();
        public IFooter CreateFooter() => new StudentFooter();
    }




  //Builder
    public class FullMessage
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }

        public string GetFullText()
        {
            return $"{Header}\n{Body}\n{Footer}";
        }
    }


    public interface IMessageBuilder
    {
        void BuildHeader();
        void BuildBody(string bodyText);
        void BuildFooter();
        FullMessage GetResult();
    }

    public class StyledMessageBuilder : IMessageBuilder
    {
        private FullMessage _message = new FullMessage();
        private INotificationStyleFactory _styleFactory;

        public StyledMessageBuilder(INotificationStyleFactory styleFactory)
        {
            _styleFactory = styleFactory;
        }

        public void BuildHeader() => _message.Header = _styleFactory.CreateHeader().GetText();
        public void BuildBody(string bodyText) => _message.Body = bodyText;
        public void BuildFooter() => _message.Footer = _styleFactory.CreateFooter().GetText();

        public FullMessage GetResult() => _message;
    }

}
