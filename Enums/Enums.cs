using System.Reflection;

namespace AlifTestTask
{
    public static class Enums
    {
        public enum TransactionTypes : int
        {
            [EnumDescription("Снятие")] Debit = 0,
            [EnumDescription("Пополнение")] Credit = 1
        }

        public enum TransactionerrorTypes : int
        {
            [EnumDescription("Успешно")] Ok = 0,
            [EnumDescription("Ошибка пополнения")] CreditError = 1,
            [EnumDescription("Ошибка снятия")] DebitError = 2,
            [EnumDescription("Превышение баланса")] BalanceError = 3           
        }

        public enum UserTypes : int
        {
            [EnumDescription("Идентифицирован")] Identified = 0,
            [EnumDescription("Неидентифицирован")] UnnIdentified = 1
        }

        public class EnumDescription : Attribute
        {
            private readonly string text;

            public EnumDescription(string text)
            {
                this.text = text;
            }

            public string Text
            {
                get { return text; }
            }
        }

        public static string GetDescription(this Enum enumeration)
        {
            Type type = enumeration.GetType();
            MemberInfo[] memInfo = type.GetMember(enumeration.ToString());

            if (null != memInfo && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumDescription), false);
                if (null != attrs && attrs.Length > 0)
                    return ((EnumDescription)attrs[0]).Text;
            }

            return enumeration.ToString();
        }
    }
}
