using System;
using System.Reflection;

namespace BAMCIS.SimpleSmtp
{
    public class SmtpCommandAttribute : Attribute
    {
        public string Command { get; set; }
        public string Comment { get; set; }

        public override string ToString()
        {
            return this.Command;
        }
    }
}
