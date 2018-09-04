using System;
using System.Collections.Specialized;
using System.Data;

namespace CommonUtility.Message
{
    /// <summary>
    /// MailService 的摘要说明。
    /// </summary>
    public class MailService
    {
        private static readonly string MailSenderConfig = MailConfigure.MailSender;
        private static readonly string MailServerConfig = MailConfigure.MailServer;
        private static readonly string MailUserConfig = MailConfigure.MailUser;
        private static readonly string MailPasswordConfig = MailConfigure.MailPassword;

        /// <summary>
        /// 使用邮件模板发送邮件
        /// </summary>
        /// <param name="_arrMailAdds"></param>
        /// <param name="_mailTemplateId"></param>
        /// <param name="_coTags"></param>
        /// <param name="_coTagstitle"></param>
        /// <param name="IsHtml">是否是HTML格式</param>
        public static void MailSender(string[] _arrMailAdds, int _mailTemplateId, NameValueCollection _coTags, NameValueCollection _coTagstitle, bool IsHtml)
        {
            string[] to = _arrMailAdds;
            string from = MailService.MailSenderConfig;

            MailTemplate cls = new MailTemplate();
            DataSet ds = cls.GetMailTemplateById(_mailTemplateId, false);

            string subject = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_TITLE].ToString();
            string body = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_BOBY].ToString();
            string sign = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_SIGN].ToString();

            for (int i = 0; i < _coTags.Count; i++)
            {
                //替换邮件标题里的标签
                subject = subject.Replace(_coTags.AllKeys[i], _coTags[i]);

                //替换邮件模板里的标签
                body = body.Replace(_coTags.AllKeys[i], _coTags[i]);
            }
            if (IsHtml)
            {
                string bfiframe = body.Substring(0, body.IndexOf("<html"));
                string mdiframe = body.Substring(body.IndexOf("<html"), body.IndexOf("</html>") - bfiframe.Length);
                string afiframe = body.Substring(body.IndexOf("</html>"));
                string newbody = bfiframe.Replace("\n", "<br>") + mdiframe + afiframe.Replace("\n", "<br>");
                MailSenderBasic(to, from, subject, newbody);
            }

        }

       /// <summary>
        /// 使用邮件模板发送邮件
       /// </summary>
       /// <param name="_arrMailAdds"></param>
       /// <param name="_mailTemplateId"></param>
       /// <param name="_coTags"></param>
        public static void MailSender(string[] _arrMailAdds, int _mailTemplateId, NameValueCollection _coTags)
        {
            string[] to = _arrMailAdds;
            string from = MailService.MailSenderConfig;

            MailTemplate cls = new MailTemplate();
            DataSet ds = cls.GetMailTemplateById(_mailTemplateId, false);

            string subject = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_TITLE].ToString();
            string body = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_BOBY].ToString();
            string sign = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_SIGN].ToString();

            for (int i = 0; i < _coTags.Count; i++)
            {
                //替换邮件标题里的标签
                subject = subject.Replace(_coTags.AllKeys[i], _coTags[i]);

                //替换邮件模板里的标签
                body = body.Replace(_coTags.AllKeys[i], _coTags[i]);
            }

            body = body.Replace("^", " ").Replace("\n", "<br>");


            body += "<p><p><p>" + sign;
            MailSenderBasic(to, from, subject, body);
        }

        /// <summary>
        /// 使用邮件模板发送邮件(html)
        /// </summary>
        /// <param name="_arrMailAdds"></param>
        /// <param name="_mailTemplateId"></param>
        /// <param name="_coTags"></param>
        public static void MailhtmlSender(string[] _arrMailAdds, int _mailTemplateId, NameValueCollection _coTags)
        {
            string[] to = _arrMailAdds;
            string from = MailService.MailSenderConfig;

            MailTemplate cls = new MailTemplate();
            DataSet ds = cls.GetMailTemplateById(_mailTemplateId, false);

            string subject = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_TITLE].ToString();
            string body = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_BOBY].ToString();
            string sign = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_SIGN].ToString();

            for (int i = 0; i < _coTags.Count; i++)
            {
                //替换邮件标题里的标签
                subject = subject.Replace(_coTags.AllKeys[i], _coTags[i]);

                //替换邮件模板里的标签
                body = body.Replace(_coTags.AllKeys[i], _coTags[i]);
            }

            body = (body.Replace("^", " ")).Replace("\n", "<br>");


            body += "<p><p><p>" + sign;
            MailSenderBasic(to, from, subject, body);
        }

        /// <summary>
        /// 使用邮件模板发送邮件(html)
        /// </summary>
        /// <param name="_arrMailAdds"></param>
        /// <param name="_arrMailCC"></param>
        /// <param name="_mailTemplateId"></param>
        /// <param name="_coTags"></param>
        public static void MailhtmlSender(string[] _arrMailAdds,string[] _arrMailCC, int _mailTemplateId, NameValueCollection _coTags)
        {
            string[] to = _arrMailAdds;
            string[] cc = _arrMailCC;
            string from = MailService.MailSenderConfig;

            MailTemplate cls = new MailTemplate();
            DataSet ds = cls.GetMailTemplateById(_mailTemplateId, false);

            string subject = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_TITLE].ToString();
            string body = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_BOBY].ToString();
            string sign = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_SIGN].ToString();

            for (int i = 0; i < _coTags.Count; i++)
            {
                //替换邮件标题里的标签
                subject = subject.Replace(_coTags.AllKeys[i], _coTags[i]);

                //替换邮件模板里的标签
                body = body.Replace(_coTags.AllKeys[i], _coTags[i]);
            }

            body = (body.Replace("^", " ")).Replace("\n", "<br>");


            body += "<p><p><p>" + sign;
            MailSenderBasic(to,cc, from, subject, body);
        }


    /// <summary>
        /// 使用邮件模板发送邮件(html)
    /// </summary>
    /// <param name="_arrMailAdds"></param>
    /// <param name="_mailTemplateId"></param>
    /// <param name="_coTags"></param>
        public static void MailhtmlSenderForSubject(string[] _arrMailAdds, int _mailTemplateId, NameValueCollection _coTags)
        {
            string[] to = _arrMailAdds;
            string from = MailService.MailSenderConfig;

            MailTemplate cls = new MailTemplate();
            DataSet ds = cls.GetMailTemplateById(_mailTemplateId, false);

            string subject = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_TITLE].ToString();
            string body = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_BOBY].ToString();
            string sign = ds.Tables[MailTemplate.TABLE_MAIL_TEMP_NAME].Rows[0][MailTemplate.COLUMN_MAIL_SIGN].ToString();

            //替换邮件模板里的标签
            for (int i = 0; i < _coTags.Count; i++)
            {
                subject = subject.Replace(_coTags.AllKeys[i], _coTags[i]);
                body = body.Replace(_coTags.AllKeys[i], _coTags[i]);
            }

            body = (body.Replace("^", " ")).Replace("\n", "<br>");
            subject = (subject.Replace("^", " ")).Replace("\n", "<br>");

            body += "<p><p><p>" + sign;
            MailSenderBasic(to, from, subject, body);
        }

        
        /// <summary>
        /// 邮件发送:基础方法
        /// </summary>
        /// <param name="_To">收件人地址(字符串数组)</param>
        /// <param name="_From">发件人地址(默认为web.config中配置的发件人)</param>
        /// <param name="_Subject">主题</param>
        /// <param name="_Body">邮件内容</param>     
        public static void MailSenderBasic(string[] _To, string _From, string _Subject, string _Body)
        {
             string errMessage = string.Empty;
            try
            {
                if (_From == null || _From.Length == 0)
                {
                    _From = MailService.MailSenderConfig;
                }

                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient(MailService.MailServerConfig);
                System.Net.NetworkCredential nwc = new System.Net.NetworkCredential(MailService.MailUserConfig, MailService.MailPasswordConfig);
                smtpClient.Credentials = nwc;
                smtpClient.EnableSsl = false;
                errMessage = "begin send mailMessage________";
                errMessage += "From:" + _From;
                errMessage += "________To[0]:" + _To[0];

                if (_Body != "")
                {
                    _Body = "<p style=\"" + MailConfigure.MailFontType + "\">" + _Body + "</p>";
                }

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage(MailService.MailSenderConfig, _To[0], _Subject, _Body);
                for (int i = 1; i < _To.Length; i++)
                {
                    mailMessage.To.Add(_To[i]);
                    errMessage += "________To[" + i.ToString() + "]:" + _To[0];
                }
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
                 

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                errMessage += "________" + ex.ToString();
                throw ex;
            }
        }

        /// <summary>
        /// 邮件发送:基础方法
        /// </summary>
        /// <param name="_To">收件人地址(字符串数组)</param>
        /// <param name="_CC">抄送人地址(字符串数组)</param>
        /// <param name="_From">发件人地址(默认为web.config中配置的发件人)</param>
        /// <param name="_Subject">主题</param>
        /// <param name="_Body">邮件内容</param>     
        public static void MailSenderBasic(string[] _To,string[] _Cc, string _From, string _Subject, string _Body)
        {
            string errMessage = string.Empty;
            try
            {
                if (_From == null || _From.Length == 0)
                {
                    _From = MailService.MailSenderConfig;
                }

                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient(MailService.MailServerConfig);
                System.Net.NetworkCredential nwc = new System.Net.NetworkCredential(MailService.MailUserConfig, MailService.MailPasswordConfig);
                smtpClient.Credentials = nwc;
                smtpClient.EnableSsl = false;
                errMessage = "begin send mailMessage________";
                errMessage += "From:" + _From;
                errMessage += "________To[0]:" + _To[0];

                if (_Body != "")
                {
                    _Body = "<p style=\"" + MailConfigure.MailFontType + "\">" + _Body + "</p>";
                }

                System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage(MailService.MailSenderConfig, _To[0], _Subject, _Body);
                for (int i = 1; i < _To.Length; i++)
                {
                    mailMessage.To.Add(_To[i]);
                    errMessage += "________To[" + i.ToString() + "]:" + _To[0];
                }
                for (int i = 0; i < _Cc.Length; i++)
                {
                    mailMessage.CC.Add(_Cc[i]);
                    errMessage += "________Cc[" + i.ToString() + "]:" + _Cc[0];
                }
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = System.Net.Mail.MailPriority.Normal;


                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                errMessage += "________" + ex.ToString();
                throw ex;
            }
        }
    }
}
