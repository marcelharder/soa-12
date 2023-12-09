namespace api.Helpers
{
   public static class EmailBody
   {
       public static string EmailStringBody(string email, string token) {
           return $@"
           <html>
   
   <head></head>
   
   <body style='margin:0;padding:0;font-family:Arial, Helvetica, sans-serif;'>
      <div style='height:500px; background: linear-gradient(to top, #c9c9ff 50%, #6e6ef6 90%); width:400px; padding:30px;'>
      <div></div>
      <div>
         <h1>Reset your password</h1>
         <hr>
         <p>You're receiving this email because you requested a password reset on your surgical outcomes account</p>
         <p>Please click the button to choose a new password</p>
         <a href='http://localhost:4200/#/resetpassword?token={token}&email={email}' target='_blank' style='background: #c9c9ff;
         color:white;border-radius: 4px;display: block;margin:0 auto;width:50%;text-align: center;text-decoration: none;'>
         Reset your password</a>
         <br><br>
         <p>Kind regards,</p>
         <p>The Surgical Outcomes Team</p>
      </div>
      <div></div>
   </div>
   </body>
   
   </html>
           ";
        }
       
   }
   
}