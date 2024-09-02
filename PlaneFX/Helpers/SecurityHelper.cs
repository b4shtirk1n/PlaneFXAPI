using System.Security.Cryptography;
using System.Text;

namespace PlaneFX.Helpers
{
	public class SecurityHelper
	{
		public static string GenerateToken(in string tg)
			=> Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes($"{tg}PlaneFX")));
	}
}