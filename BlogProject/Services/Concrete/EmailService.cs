using BlogProject.Models;
using BlogProject.Services.Abstract;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BlogProject.Services.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendResetPasswordEmailAsync(string resetEmailLink, string receiverEmail)
        {
            var smtpClient = new SmtpClient();

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Host = _emailSettings.Host;
            smtpClient.Port = _emailSettings.Port;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_emailSettings.Email, "kayas.com");
            mailMessage.To.Add(receiverEmail);
            mailMessage.Subject = "Your request to change your password";
            mailMessage.Body = @$"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html dir=""ltr"" xmlns=""http://www.w3.org/1999/xhtml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" lang=""en"">
 <head>
  <meta charset=""UTF-8"">
  <meta content=""width=device-width, initial-scale=1"" name=""viewport"">
  <meta name=""x-apple-disable-message-reformatting"">
  <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
  <meta content=""telephone=no"" name=""format-detection"">
  <title>New Message</title><!--[if (mso 16)]>
    <style type=""text/css"">
    a {{text-decoration: none;}}
    </style>
    <![endif]--><!--[if gte mso 9]><style>sup {{ font-size: 100% !important; }}</style><![endif]--><!--[if gte mso 9]>
<noscript>
         <xml>
           <o:OfficeDocumentSettings>
           <o:AllowPNG></o:AllowPNG>
           <o:PixelsPerInch>96</o:PixelsPerInch>
           </o:OfficeDocumentSettings>
         </xml>
      </noscript>
<![endif]--><!--[if !mso]><!-- -->
  <link href=""https://fonts.googleapis.com/css2?family=Orbitron&display=swap"" rel=""stylesheet""><!--<![endif]--><!--[if mso]><xml>
    <w:WordDocument xmlns:w=""urn:schemas-microsoft-com:office:word"">
      <w:DontUseAdvancedTypographyReadingMail/>
    </w:WordDocument>
    </xml><![endif]-->
  <style type=""text/css"">
.rollover:hover .rollover-first {{
  max-height:0px!important;
  display:none!important;
}}
.rollover:hover .rollover-second {{
  max-height:none!important;
  display:block!important;
}}
.rollover span {{
  font-size:0px;
}}
u + .body img ~ div div {{
  display:none;
}}
#outlook a {{
  padding:0;
}}
span.MsoHyperlink,
span.MsoHyperlinkFollowed {{
  color:inherit;
  mso-style-priority:99;
}}
a.es-button {{
  mso-style-priority:100!important;
  text-decoration:none!important;
}}
a[x-apple-data-detectors],
#MessageViewBody a {{
  color:inherit!important;
  text-decoration:none!important;
  font-size:inherit!important;
  font-family:inherit!important;
  font-weight:inherit!important;
  line-height:inherit!important;
}}
.es-desk-hidden {{
  display:none;
  float:left;
  overflow:hidden;
  width:0;
  max-height:0;
  line-height:0;
  mso-hide:all;
}}
.es-button-border:hover {{
  border-color:#26C6DA #26C6DA #26C6DA #26C6DA!important;
  background:#58dfec!important;
}}
.es-button-border:hover a.es-button,
.es-button-border:hover button.es-button {{
  background:#58dfec!important;
}}
@media only screen and (max-width:600px) {{.es-m-p0r {{ padding-right:0px!important }} .es-m-p20b {{ padding-bottom:20px!important }} .es-p-default {{ }} *[class=""gmail-fix""] {{ display:none!important }} p, a {{ line-height:150%!important }} h1, h1 a {{ line-height:120%!important }} h2, h2 a {{ line-height:120%!important }} h3, h3 a {{ line-height:120%!important }} h4, h4 a {{ line-height:120%!important }} h5, h5 a {{ line-height:120%!important }} h6, h6 a {{ line-height:120%!important }} .es-header-body p {{ }} .es-content-body p {{ }} .es-footer-body p {{ }} .es-infoblock p {{ }} h1 {{ font-size:30px!important; text-align:center }} h2 {{ font-size:24px!important; text-align:left }} h3 {{ font-size:20px!important; text-align:left }} h4 {{ font-size:24px!important; text-align:left }} h5 {{ font-size:20px!important; text-align:left }} h6 {{ font-size:16px!important; text-align:left }} .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a {{ font-size:30px!important }} .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a {{ font-size:24px!important }} .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a {{ font-size:20px!important }} .es-header-body h4 a, .es-content-body h4 a, .es-footer-body h4 a {{ font-size:24px!important }} .es-header-body h5 a, .es-content-body h5 a, .es-footer-body h5 a {{ font-size:20px!important }} .es-header-body h6 a, .es-content-body h6 a, .es-footer-body h6 a {{ font-size:16px!important }} .es-menu td a {{ font-size:14px!important }} .es-header-body p, .es-header-body a {{ font-size:14px!important }} .es-content-body p, .es-content-body a {{ font-size:14px!important }} .es-footer-body p, .es-footer-body a {{ font-size:14px!important }} .es-infoblock p, .es-infoblock a {{ font-size:12px!important }} .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3, .es-m-txt-c h4, .es-m-txt-c h5, .es-m-txt-c h6 {{ text-align:center!important }} .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3, .es-m-txt-r h4, .es-m-txt-r h5, .es-m-txt-r h6 {{ text-align:right!important }} .es-m-txt-j, .es-m-txt-j h1, .es-m-txt-j h2, .es-m-txt-j h3, .es-m-txt-j h4, .es-m-txt-j h5, .es-m-txt-j h6 {{ text-align:justify!important }} .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3, .es-m-txt-l h4, .es-m-txt-l h5, .es-m-txt-l h6 {{ text-align:left!important }} .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img {{ display:inline!important }} .es-m-txt-r .rollover:hover .rollover-second, .es-m-txt-c .rollover:hover .rollover-second, .es-m-txt-l .rollover:hover .rollover-second {{ display:inline!important }} .es-m-txt-r .rollover span, .es-m-txt-c .rollover span, .es-m-txt-l .rollover span {{ line-height:0!important; font-size:0!important; display:block }} .es-spacer {{ display:inline-table }} a.es-button, button.es-button {{ font-size:18px!important; padding:10px 20px 10px 20px!important; line-height:120%!important }} a.es-button, button.es-button, .es-button-border {{ display:inline-block!important }} .es-m-fw, .es-m-fw.es-fw, .es-m-fw .es-button {{ display:block!important }} .es-m-il, .es-m-il .es-button, .es-social, .es-social td, .es-menu {{ display:inline-block!important }} .es-adaptive table, .es-left, .es-right {{ width:100%!important }} .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header {{ width:100%!important; max-width:600px!important }} .adapt-img {{ width:100%!important; height:auto!important }} .es-mobile-hidden, .es-hidden {{ display:none!important }} .es-desk-hidden {{ width:auto!important; overflow:visible!important; float:none!important; max-height:inherit!important; line-height:inherit!important }} tr.es-desk-hidden {{ display:table-row!important }} table.es-desk-hidden {{ display:table!important }} td.es-desk-menu-hidden {{ display:table-cell!important }} .es-menu td {{ width:1%!important }} table.es-table-not-adapt, .esd-block-html table {{ width:auto!important }} .h-auto {{ height:auto!important }} }}
@media screen and (max-width:384px) {{.mail-message-content {{ width:414px!important }} }}
</style>
 </head>
 <body class=""body"" style=""width:100%;height:100%;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0"">
  <div dir=""ltr"" class=""es-wrapper-color"" lang=""en"" style=""background-color:#07023C""><!--[if gte mso 9]>
			<v:background xmlns:v=""urn:schemas-microsoft-com:vml"" fill=""t"">
				<v:fill type=""tile"" color=""#07023c""></v:fill>
			</v:background>
		<![endif]-->
   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" class=""es-wrapper"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#07023C"">
     <tr>
      <td valign=""top"" style=""padding:0;Margin:0"">
       <table cellspacing=""0"" cellpadding=""0"" align=""center"" class=""es-content"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important"">
         <tr>
          <td align=""center"" style=""padding:0;Margin:0"">
           <table cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"" class=""es-content-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#ffffff;background-repeat:no-repeat;width:600px;background-image:url(https://fkreoee.stripocdn.email/content/guids/CABINET_0e8fbb6adcc56c06fbd3358455fdeb41/images/vector_0Ia.png);background-position:center center"" background=""https://fkreoee.stripocdn.email/content/guids/CABINET_0e8fbb6adcc56c06fbd3358455fdeb41/images/vector_0Ia.png"" role=""none"">
             <tr>
              <td align=""left"" style=""Margin:0;padding-top:20px;padding-right:20px;padding-bottom:10px;padding-left:20px"">
               <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                 <tr>
                  <td valign=""top"" align=""center"" class=""es-m-p0r"" style=""padding:0;Margin:0;width:560px"">
                   <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                     <tr>
                      <td align=""center"" style=""padding:0;Margin:0""><h3 style=""Margin:0;font-family:Orbitron, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:28px;font-style:normal;font-weight:bold;line-height:33.6px;color:#10054D""><strong></strong></h3></td>
                     </tr>
                   </table></td>
                 </tr>
               </table></td>
             </tr>
             <tr>
              <td align=""left"" style=""Margin:0;padding-right:20px;padding-left:20px;padding-top:30px;padding-bottom:30px"">
               <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                 <tr>
                  <td valign=""top"" align=""center"" class=""es-m-p0r es-m-p20b"" style=""padding:0;Margin:0;width:560px"">
                   <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                     <tr>
                      <td align=""center"" style=""padding:0;Margin:0""><h1 style=""Margin:0;font-family:Orbitron, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:44px;font-style:normal;font-weight:bold;line-height:52.8px;color:#10054D"">&nbsp;We got a request to reset your&nbsp;password</h1></td>
                     </tr>
                     <tr>
                      <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:15px;font-size:0px""><a target=""_blank"" href=""https://viewstripo.email"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#26C6DA;font-size:14px""><img src=""https://fkreoee.stripocdn.email/content/guids/CABINET_dee64413d6f071746857ca8c0f13d696/images/852converted_1x3.png"" alt="""" height=""300"" class=""adapt-img"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></a></td>
                     </tr>
                     <tr>
                      <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:10px""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px"">&nbsp;Forgot your password? No problem - it happens to everyone!</p></td>
                     </tr>
                     <tr>
                      <td align=""center"" style=""padding:0;Margin:0;padding-top:15px;padding-bottom:15px""><span class=""es-button-border"" style=""border-style:solid;border-color:#26C6DA;background:#26C6DA;border-width:4px;display:inline-block;border-radius:10px;width:auto""><a href=""{resetEmailLink}"" target=""_blank"" class=""es-button"" style=""mso-style-priority:100 !important;text-decoration:none !important;mso-line-height-rule:exactly;color:#FFFFFF;font-size:20px;padding:10px 25px 10px 30px;display:inline-block;background:#26C6DA;border-radius:10px;font-family:arial, 'helvetica neue', helvetica, sans-serif;font-weight:normal;font-style:normal;line-height:24px;width:auto;text-align:center;letter-spacing:0;mso-padding-alt:0;mso-border-alt:10px solid #26C6DA""> Reset Your Password</a></span></td>
                     </tr>
                     <tr>
                      <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:10px""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px"">If you ignore this message, your password won't be changed.</p></td>
                     </tr>
                   </table></td>
                 </tr>
               </table></td>
             </tr>
           </table></td>
         </tr>
       </table>
       <table cellpadding=""0"" cellspacing=""0"" align=""center"" class=""es-content"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important"">
         <tr>
          <td align=""center"" style=""padding:0;Margin:0"">
           <table bgcolor=""#10054D"" align=""center"" cellpadding=""0"" cellspacing=""0"" class=""es-content-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#10054d;width:600px"" role=""none"">
             <tr>
              <td align=""left"" background=""https://fkreoee.stripocdn.email/content/guids/CABINET_0e8fbb6adcc56c06fbd3358455fdeb41/images/vector_sSY.png"" style=""Margin:0;padding-right:20px;padding-left:20px;padding-top:35px;padding-bottom:35px;background-image:url(https://fkreoee.stripocdn.email/content/guids/CABINET_0e8fbb6adcc56c06fbd3358455fdeb41/images/vector_sSY.png);background-repeat:no-repeat;background-position:left center""><!--[if mso]><table style=""width:560px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:69px"" valign=""top""><![endif]-->
               <table cellpadding=""0"" cellspacing=""0"" align=""left"" class=""es-left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                 <tr>
                  <td align=""left"" class=""es-m-p20b"" style=""padding:0;Margin:0;width:69px"">
                   <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                     <tr>
                      <td align=""center"" class=""es-m-txt-l"" style=""padding:0;Margin:0;font-size:0px""><a target=""_blank"" href=""https://viewstripo.email"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#26C6DA;font-size:14px""><img src=""https://fkreoee.stripocdn.email/content/guids/CABINET_dee64413d6f071746857ca8c0f13d696/images/group_118_lFL.png"" alt="""" width=""69"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></a></td>
                     </tr>
                   </table></td>
                 </tr>
               </table><!--[if mso]></td><td style=""width:20px""></td><td style=""width:471px"" valign=""top""><![endif]-->
               <table cellpadding=""0"" cellspacing=""0"" align=""right"" class=""es-right"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right"">
                 <tr>
                  <td align=""left"" style=""padding:0;Margin:0;width:471px"">
                   <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                     <tr>
                      <td align=""left"" style=""padding:0;Margin:0""><h3 style=""Margin:0;font-family:Orbitron, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:28px;font-style:normal;font-weight:bold;line-height:33.6px;color:#ffffff""><b>Real people. Here to help.</b></h3></td>
                     </tr>
                     <tr>
                      <td align=""left"" style=""padding:0;Margin:0;padding-top:10px;padding-bottom:5px""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#ffffff;font-size:14px"">Have a question? Give us a call at <strong><a href=""tel:(000)1234567899"" target=""_blank"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#26C6DA;font-size:14px"">+90 (543) 872 61 77</a> </strong>&nbsp;to chat with a Customer Success representative.</p></td>
                     </tr>
                   </table></td>
                 </tr>
               </table><!--[if mso]></td></tr></table><![endif]--></td>
             </tr>
           </table></td>
         </tr>
       </table>
       <table cellpadding=""0"" cellspacing=""0"" align=""center"" class=""es-footer"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top"">
         <tr>
          <td align=""center"" style=""padding:0;Margin:0"">
           <table bgcolor=""#ffffff"" align=""center"" cellpadding=""0"" cellspacing=""0"" class=""es-footer-body"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px"">
             <tr>
              <td align=""left"" style=""padding:0;Margin:0;padding-right:20px;padding-left:20px;padding-top:30px"">
               <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                 <tr>
                  <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px"">
                   <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                     <tr>
                      <td align=""center"" style=""padding:0;Margin:0""><h2 style=""Margin:0;font-family:Orbitron, sans-serif;mso-line-height-rule:exactly;letter-spacing:0;font-size:36px;font-style:normal;font-weight:bold;line-height:43.2px;color:#10054D"">Is this newsletter helpful?</h2></td>
                     </tr>
                   </table></td>
                 </tr>
               </table></td>
             </tr>
             <tr>
              <td align=""left"" class=""esdev-adapt-off"" style=""Margin:0;padding-right:20px;padding-left:20px;padding-top:30px;padding-bottom:20px"">
               <table cellpadding=""0"" cellspacing=""0"" class=""esdev-mso-table"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:560px"">
                 <tr>
                  <td valign=""top"" class=""esdev-mso-td"" style=""padding:0;Margin:0"">
                   <table cellpadding=""0"" cellspacing=""0"" align=""left"" class=""es-left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                     <tr class=""es-mobile-hidden"">
                      <td align=""left"" style=""padding:0;Margin:0;width:63px"">
                       <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                         <tr>
                          <td align=""center"" height=""40"" style=""padding:0;Margin:0""></td>
                         </tr>
                       </table></td>
                     </tr>
                   </table></td>
                  <td style=""padding:0;Margin:0;width:20px""></td>
                  <td valign=""top"" class=""esdev-mso-td"" style=""padding:0;Margin:0"">
                   <table cellpadding=""0"" cellspacing=""0"" align=""left"" class=""es-left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                     <tr>
                      <td align=""left"" style=""padding:0;Margin:0;width:63px"">
                       <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                         <tr>
                          <td align=""center"" style=""padding:0;Margin:0""><span class=""es-button-border"" style=""border-style:solid;border-color:#26C6DA;background:#26C6DA;border-width:4px;display:inline-block;border-radius:10px;width:auto""><a href=""https://viewstripo.email"" target=""_blank"" class=""es-button es-button-1637938355061"" style=""mso-style-priority:100 !important;text-decoration:none !important;mso-line-height-rule:exactly;color:#FFFFFF;font-size:16px;padding:5px 15px;display:inline-block;background:#26C6DA;border-radius:10px;font-family:arial, 'helvetica neue', helvetica, sans-serif;font-weight:normal;font-style:normal;line-height:19.2px;width:auto;text-align:center;letter-spacing:0;mso-padding-alt:0;mso-border-alt:10px solid #26C6DA"">1</a></span></td>
                         </tr>
                       </table></td>
                     </tr>
                   </table></td>
                  <td style=""padding:0;Margin:0;width:20px""></td>
                  <td valign=""top"" class=""esdev-mso-td"" style=""padding:0;Margin:0"">
                   <table cellpadding=""0"" cellspacing=""0"" align=""left"" class=""es-left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                     <tr>
                      <td align=""left"" style=""padding:0;Margin:0;width:63px"">
                       <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                         <tr>
                          <td align=""center"" style=""padding:0;Margin:0""><span class=""es-button-border"" style=""border-style:solid;border-color:#26C6DA;background:#26C6DA;border-width:4px;display:inline-block;border-radius:10px;width:auto""><a href=""https://viewstripo.email"" target=""_blank"" class=""es-button es-button-1637938414709"" style=""mso-style-priority:100 !important;text-decoration:none !important;mso-line-height-rule:exactly;color:#FFFFFF;font-size:16px;padding:5px 15px;display:inline-block;background:#26C6DA;border-radius:10px;font-family:arial, 'helvetica neue', helvetica, sans-serif;font-weight:normal;font-style:normal;line-height:19.2px;width:auto;text-align:center;letter-spacing:0;mso-padding-alt:0;mso-border-alt:10px solid #26C6DA"">2</a></span></td>
                         </tr>
                       </table></td>
                     </tr>
                   </table></td>
                  <td style=""padding:0;Margin:0;width:20px""></td>
                  <td valign=""top"" class=""esdev-mso-td"" style=""padding:0;Margin:0"">
                   <table cellpadding=""0"" cellspacing=""0"" align=""left"" class=""es-left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                     <tr>
                      <td align=""left"" style=""padding:0;Margin:0;width:63px"">
                       <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                         <tr>
                          <td align=""center"" style=""padding:0;Margin:0""><span class=""es-button-border"" style=""border-style:solid;border-color:#26C6DA;background:#26C6DA;border-width:4px;display:inline-block;border-radius:10px;width:auto""><a href=""https://viewstripo.email"" target=""_blank"" class=""es-button es-button-1637938422665"" style=""mso-style-priority:100 !important;text-decoration:none !important;mso-line-height-rule:exactly;color:#FFFFFF;font-size:16px;padding:5px 15px;display:inline-block;background:#26C6DA;border-radius:10px;font-family:arial, 'helvetica neue', helvetica, sans-serif;font-weight:normal;font-style:normal;line-height:19.2px;width:auto;text-align:center;letter-spacing:0;mso-padding-alt:0;mso-border-alt:10px solid #26C6DA"">3</a></span></td>
                         </tr>
                       </table></td>
                     </tr>
                   </table></td>
                  <td style=""padding:0;Margin:0;width:20px""></td>
                  <td valign=""top"" class=""esdev-mso-td"" style=""padding:0;Margin:0"">
                   <table cellpadding=""0"" cellspacing=""0"" align=""left"" class=""es-left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                     <tr>
                      <td align=""left"" style=""padding:0;Margin:0;width:63px"">
                       <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                         <tr>
                          <td align=""center"" style=""padding:0;Margin:0""><span class=""es-button-border"" style=""border-style:solid;border-color:#26C6DA;background:#26C6DA;border-width:4px;display:inline-block;border-radius:10px;width:auto""><a href=""https://viewstripo.email"" target=""_blank"" class=""es-button es-button-1637938430832"" style=""mso-style-priority:100 !important;text-decoration:none !important;mso-line-height-rule:exactly;color:#FFFFFF;font-size:16px;padding:5px 15px;display:inline-block;background:#26C6DA;border-radius:10px;font-family:arial, 'helvetica neue', helvetica, sans-serif;font-weight:normal;font-style:normal;line-height:19.2px;width:auto;text-align:center;letter-spacing:0;mso-padding-alt:0;mso-border-alt:10px solid #26C6DA"">4</a></span></td>
                         </tr>
                       </table></td>
                     </tr>
                   </table></td>
                  <td style=""padding:0;Margin:0;width:20px""></td>
                  <td valign=""top"" class=""esdev-mso-td"" style=""padding:0;Margin:0"">
                   <table cellpadding=""0"" cellspacing=""0"" align=""left"" class=""es-left"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
                     <tr>
                      <td align=""left"" style=""padding:0;Margin:0;width:63px"">
                       <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                         <tr>
                          <td align=""center"" style=""padding:0;Margin:0""><span class=""es-button-border"" style=""border-style:solid;border-color:#26C6DA;background:#26C6DA;border-width:4px;display:inline-block;border-radius:10px;width:auto""><a href=""https://viewstripo.email"" target=""_blank"" class=""es-button es-button-1637938437176"" style=""mso-style-priority:100 !important;text-decoration:none !important;mso-line-height-rule:exactly;color:#FFFFFF;font-size:16px;padding:5px 15px;display:inline-block;background:#26C6DA;border-radius:10px;font-family:arial, 'helvetica neue', helvetica, sans-serif;font-weight:normal;font-style:normal;line-height:19.2px;width:auto;text-align:center;letter-spacing:0;mso-padding-alt:0;mso-border-alt:10px solid #26C6DA"">5</a></span></td>
                         </tr>
                       </table></td>
                     </tr>
                   </table></td>
                  <td style=""padding:0;Margin:0;width:20px""></td>
                  <td valign=""top"" class=""esdev-mso-td"" style=""padding:0;Margin:0"">
                   <table cellpadding=""0"" cellspacing=""0"" align=""right"" class=""es-right"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right"">
                     <tr class=""es-mobile-hidden"">
                      <td align=""left"" style=""padding:0;Margin:0;width:62px"">
                       <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                         <tr>
                          <td align=""center"" height=""40"" style=""padding:0;Margin:0""></td>
                         </tr>
                       </table></td>
                     </tr>
                   </table></td>
                 </tr>
               </table></td>
             </tr>
             <tr>
              <td align=""left"" style=""Margin:0;padding-right:20px;padding-left:20px;padding-bottom:30px;padding-top:10px"">
               <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                 <tr>
                  <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px"">
                   <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                     <tr>
                      <td align=""center"" style=""padding:0;Margin:0""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:21px;letter-spacing:0;color:#333333;font-size:14px"">1 —&nbsp;Not at all&nbsp;helpful&nbsp;😟<br>5&nbsp;—&nbsp;Extremely&nbsp;helpful&nbsp;😊</p></td>
                     </tr>
                   </table></td>
                 </tr>
               </table></td>
             </tr>
           </table></td>
         </tr>
       </table>
       <table cellpadding=""0"" cellspacing=""0"" align=""center"" class=""es-footer"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;width:100%;table-layout:fixed !important;background-color:transparent;background-repeat:repeat;background-position:center top"">
         <tr>
          <td align=""center"" style=""padding:0;Margin:0"">
           <table align=""center"" cellpadding=""0"" cellspacing=""0"" class=""es-footer-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px"" role=""none"">
             <tr>
              <td align=""left"" style=""Margin:0;padding-top:20px;padding-right:20px;padding-left:20px;padding-bottom:20px"">
               <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                 <tr>
                  <td align=""left"" style=""padding:0;Margin:0;width:560px"">
                   <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                     <tr>
                      <td style=""padding:0;Margin:0"">
                       <table cellpadding=""0"" cellspacing=""0"" width=""100%"" class=""es-menu"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                         <tr class=""links"">
                          <td align=""center"" valign=""top"" width=""50.00%"" style=""Margin:0;border:0;padding-bottom:10px;padding-top:10px;padding-right:5px;padding-left:5px"">
                           <div style=""vertical-align:middle;display:block"">
                            <a target=""_blank"" href=""https://medium.com/@gorkemkayas"" style=""mso-line-height-rule:exactly;text-decoration:none;font-family:arial, 'helvetica neue', helvetica, sans-serif;display:block;color:#ffffff;font-size:14px"">News from owner</a>
                           </div></td>
                          <td align=""center"" valign=""top"" width=""50.00%"" style=""Margin:0;border:0;padding-bottom:10px;padding-top:10px;padding-right:5px;padding-left:5px"">
                           <div style=""vertical-align:middle;display:block"">
                            <a target=""_blank"" href=""https://www.linkedin.com/in/gorkemkayas/"" style=""mso-line-height-rule:exactly;text-decoration:none;font-family:arial, 'helvetica neue', helvetica, sans-serif;display:block;color:#ffffff;font-size:14px"">Owner Linkedin</a>
                           </div></td>
                         </tr>
                       </table></td>
                     </tr>
                     <tr>
                      <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:10px;font-size:0"">
                       <table cellpadding=""0"" cellspacing=""0"" class=""es-table-not-adapt es-social"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                         <tr>
                          <td align=""center"" valign=""top"" style=""padding:0;Margin:0;padding-right:20px""><a target=""_blank"" href=""https://x.com/gorkemkayaz"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#FFFFFF;font-size:14px""><img title=""X"" src=""https://fkreoee.stripocdn.email/content/assets/img/social-icons/square-colored/x-square-colored.png"" alt=""X"" width=""32"" height=""32"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></a></td>
                          <td align=""center"" valign=""top"" style=""padding:0;Margin:0;padding-right:20px""><a target=""_blank"" href=""https://www.instagram.com/gorkemkayas/"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#FFFFFF;font-size:14px""><img title=""Instagram"" src=""https://fkreoee.stripocdn.email/content/assets/img/social-icons/square-colored/instagram-square-colored.png"" alt=""Inst"" width=""32"" height=""32"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></a></td>
                         </tr>
                       </table></td>
                     </tr>
                     <tr>
                      <td align=""center"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:10px""><p style=""Margin:0;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:18px;letter-spacing:0;color:#ffffff;font-size:12px"">You are receiving this email because you have visited our site or asked us about the regular newsletter. Make sure our messages get to your Inbox (and not your bulk or junk folders).<br><a target=""_blank"" href=""https://viewstripo.email"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#FFFFFF;font-size:12px"">Privacy police</a> | <a target=""_blank"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#FFFFFF;font-size:12px"" href="""">Unsubscribe</a></p></td>
                     </tr>
                   </table></td>
                 </tr>
               </table></td>
             </tr>
             <tr>
              <td align=""left"" style=""padding:20px;Margin:0"">
               <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""none"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                 <tr>
                  <td align=""center"" valign=""top"" style=""padding:0;Margin:0;width:560px"">
                   <table cellpadding=""0"" cellspacing=""0"" width=""100%"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
                     <tr>
                      <td align=""center"" class=""es-infoblock made_with"" style=""padding:0;Margin:0;font-size:0""><a target=""_blank"" href=""https://viewstripo.email/?utm_source=templates&utm_medium=email&utm_campaign=product_update_1&utm_content=it_happens_to_everyone"" style=""mso-line-height-rule:exactly;text-decoration:underline;color:#CCCCCC;font-size:12px""><img src=""https://fkreoee.stripocdn.email/content/guids/CABINET_09023af45624943febfa123c229a060b/images/7911561025989373.png"" alt="""" width=""125"" style=""display:block;font-size:14px;border:0;outline:none;text-decoration:none""></a></td>
                     </tr>
                   </table></td>
                 </tr>
               </table></td>
             </tr>
           </table></td>
         </tr>
       </table></td>
     </tr>
   </table>
  </div>
 </body>
</html>";

            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendPasswordChangedNotificationAsync(string subject, string receiverEmail)
        {
            var smtpClient = new SmtpClient();

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Host = _emailSettings.Host;
            smtpClient.Port = _emailSettings.Port;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_emailSettings.Email, "kayas.com");
            mailMessage.To.Add(receiverEmail);
            mailMessage.Subject = $"{subject}";
            mailMessage.Body = @$"<!DOCTYPE html>
<html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" lang=""en"">

<head>
	<title></title>
	<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
	<meta name=""viewport"" content=""width=device-width, initial-scale=1.0""><!--[if mso]><xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]--><!--[if !mso]><!-->
	<link href=""https://fonts.googleapis.com/css2?family=Montserrat:wght@100;200;300;400;500;600;700;800;900"" rel=""stylesheet"" type=""text/css""><!--<![endif]-->
	<style>
		* {{
			box-sizing: border-box;
		}}

		body {{
			margin: 0;
			padding: 0;
		}}

		a[x-apple-data-detectors] {{
			color: inherit !important;
			text-decoration: inherit !important;
		}}

		#MessageViewBody a {{
			color: inherit;
			text-decoration: none;
		}}

		p {{
			line-height: inherit
		}}

		.desktop_hide,
		.desktop_hide table {{
			mso-hide: all;
			display: none;
			max-height: 0px;
			overflow: hidden;
		}}

		.image_block img+div {{
			display: none;
		}}

		sup,
		sub {{
			font-size: 75%;
			line-height: 0;
		}}

		@media (max-width:660px) {{
			.desktop_hide table.icons-inner {{
				display: inline-block !important;
			}}

			.icons-inner {{
				text-align: center;
			}}

			.icons-inner td {{
				margin: 0 auto;
			}}

			.mobile_hide {{
				display: none;
			}}

			.row-content {{
				width: 100% !important;
			}}

			.stack .column {{
				width: 100%;
				display: block;
			}}

			.mobile_hide {{
				min-height: 0;
				max-height: 0;
				max-width: 0;
				overflow: hidden;
				font-size: 0px;
			}}

			.desktop_hide,
			.desktop_hide table {{
				display: table !important;
				max-height: none !important;
			}}
		}}
	</style><!--[if mso ]><style>sup, sub {{ font-size: 100% !important; }} sup {{ mso-text-raise:10% }} sub {{ mso-text-raise:-10% }}</style> <![endif]-->
</head>

<body class=""body"" style=""background-color: #f8f8f9; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;"">
	<table class=""nl-container"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #f8f8f9;"">
		<tbody>
			<tr>
				<td>
					<table class=""row row-1"" align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #1aa19c;"">
						<tbody>
							<tr>
								<td>
									<table class=""row-content stack"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; background-color: #1aa19c; width: 640px; margin: 0 auto;"" width=""640"">
										<tbody>
											<tr>
												<td class=""column column-1"" width=""100%"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top;"">
													<table class=""divider_block block-1"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt;"">
														<tr>
															<td class=""pad"">
																<div class=""alignment"" align=""center"">
																	<table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" width=""100%"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt;"">
																		<tr>
																			<td class=""divider_inner"" style=""font-size: 1px; line-height: 1px; border-top: 4px solid #1AA19C;""><span style=""word-break: break-word;"">&#8202;</span></td>
																		</tr>
																	</table>
																</div>
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</tbody>
									</table>
								</td>
							</tr>
						</tbody>
					</table>
					<table class=""row row-2"" align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt;"">
						<tbody>
							<tr>
								<td>
									<table class=""row-content stack"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #fff; color: #000000; width: 640px; margin: 0 auto;"" width=""640"">
										<tbody>
											<tr>
												<td class=""column column-1"" width=""100%"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top;"">
													<table class=""image_block block-1"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt;"">
														<tr>
															<td class=""pad"" style=""width:100%;"">
																<div class=""alignment"" align=""center"" style=""line-height:10px"">
																	<div style=""max-width: 640px;""><a style=""outline:none"" tabindex=""-1""><img src=""https://d1oco4z2z1fhwp.cloudfront.net/templates/default/4036/___passwordreset.gif"" style=""display: block; height: auto; border: 0; width: 100%;"" width=""640"" alt=""Image of lock &amp; key."" title=""Image of lock &amp; key."" height=""auto""></a></div>
																</div>
															</td>
														</tr>
													</table>
													<table class=""divider_block block-2"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt;"">
														<tr>
															<td class=""pad"" style=""padding-top:30px;"">
																<div class=""alignment"" align=""center"">
																	<table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" width=""100%"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt;"">
																		<tr>
																			<td class=""divider_inner"" style=""font-size: 1px; line-height: 1px; border-top: 0px solid #BBBBBB;""><span style=""word-break: break-word;"">&#8202;</span></td>
																		</tr>
																	</table>
																</div>
															</td>
														</tr>
													</table>
													<table class=""paragraph_block block-3"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;"">
														<tr>
															<td class=""pad"" style=""padding-bottom:10px;padding-left:40px;padding-right:40px;padding-top:10px;"">
																<div style=""color:#555555;font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-size:30px;line-height:120%;text-align:center;mso-line-height-alt:36px;"">
																	<p style=""margin: 0; word-break: break-word;"">You changed your password.</p>
																</div>
															</td>
														</tr>
													</table>
													<table class=""paragraph_block block-4"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;"">
														<tr>
															<td class=""pad"" style=""padding-bottom:10px;padding-left:40px;padding-right:40px;padding-top:10px;"">
																<div style=""color:#555555;font-family:Montserrat, Trebuchet MS, Lucida Grande, Lucida Sans Unicode, Lucida Sans, Tahoma, sans-serif;font-size:15px;line-height:150%;text-align:center;mso-line-height-alt:22.5px;"">
																	<p style=""margin: 0;"">This is an informational message. You have recently changed your password. <strong>If this is not yours, please contact +90 543 872 61 77</strong></p>
																</div>
															</td>
														</tr>
													</table>
													<table class=""divider_block block-5"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt;"">
														<tr>
															<td class=""pad"" style=""padding-bottom:12px;padding-top:60px;"">
																<div class=""alignment"" align=""center"">
																	<table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" width=""100%"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt;"">
																		<tr>
																			<td class=""divider_inner"" style=""font-size: 1px; line-height: 1px; border-top: 0px solid #BBBBBB;""><span style=""word-break: break-word;"">&#8202;</span></td>
																		</tr>
																	</table>
																</div>
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</tbody>
									</table>
								</td>
							</tr>
						</tbody>
					</table>
					<table class=""row row-3"" align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #ffffff;"">
						<tbody>
							<tr>
								<td>
									<table class=""row-content stack"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000; background-color: #ffffff; width: 640px; margin: 0 auto;"" width=""640"">
										<tbody>
											<tr>
												<td class=""column column-1"" width=""100%"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; padding-bottom: 5px; padding-top: 5px; vertical-align: top;"">
													<table class=""icons_block block-1"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; text-align: center; line-height: 0;"">
														<tr>
															<td class=""pad"" style=""vertical-align: middle; color: #1e0e4b; font-family: 'Inter', sans-serif; font-size: 15px; padding-bottom: 5px; padding-top: 5px; text-align: center;""><!--[if vml]><table align=""center"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""display:inline-block;padding-left:0px;padding-right:0px;mso-table-lspace: 0pt;mso-table-rspace: 0pt;""><![endif]-->
																<!--[if !vml]><!-->
																<table class=""icons-inner"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; display: inline-block; padding-left: 0px; padding-right: 0px;"" cellpadding=""0"" cellspacing=""0"" role=""presentation""><!--<![endif]-->
																	<tr>
																		<td style=""vertical-align: middle; text-align: center; padding-top: 5px; padding-bottom: 5px; padding-left: 5px; padding-right: 6px;""><a href=""http://designedwithbeefree.com/"" target=""_blank"" style=""text-decoration: none;""><img class=""icon"" alt=""Beefree Logo"" src=""https://d1oco4z2z1fhwp.cloudfront.net/assets/Beefree-logo.png"" height=""auto"" width=""34"" align=""center"" style=""display: block; height: auto; margin: 0 auto; border: 0;""></a></td>
																		<td style=""font-family: 'Inter', sans-serif; font-size: 15px; font-weight: undefined; color: #1e0e4b; vertical-align: middle; letter-spacing: undefined; text-align: center; line-height: normal;""><a href=""http://designedwithbeefree.com/"" target=""_blank"" style=""color: #1e0e4b; text-decoration: none;"">Designed with Beefree</a></td>
																	</tr>
																</table>
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</tbody>
									</table>
								</td>
							</tr>
						</tbody>
					</table>
				</td>
			</tr>
		</tbody>
	</table><!-- End -->
</body>

</html>";

            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
