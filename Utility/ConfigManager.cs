using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Escrow.BOAS.Utility
{
    public static class ConfigManager
    {
        public static string Email_FromAddress
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["Email_FromAddress"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string Email_FromDisplayName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["Email_FromDisplayName"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string Email_User
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["Email_User"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string Email_Password
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["Email_Password"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string cdbl_file_path
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["cdbl_file_path"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string bk_file_path
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["bk_file_path"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string payInOut_file_path
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["payInOut_file_path"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string trade_export_file_path
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["trade_export_file_path"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string report_server_url
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["report_server_url"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string auto_generated_report_path
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["auto_generated_report_path"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string report_network_user_id
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["report_network_user_id"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string report_network_password
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["report_network_password"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string balance_check_type
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["balance_check_type"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string sms_excel_file_path
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["sms_excel_file_path"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string smtp_address
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["smtp_address"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }
        public static string email_from
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["email_from"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }


        public static string email_password
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["email_password"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string report_path
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["report_path"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string smtp_timeout
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["smtp_timeout"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string mail_log_file_path
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["mail_log_file_path"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }

        public static string is_digits_enabled_menu 
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["is_digits_enabled_menu"];
                }
                catch (Exception ex)
                {
                    throw new Exception("Configuration Setting Error: " + ex.Message);
                }
            }
        }
    }
}