using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Abstractions
{
	public interface ISanitizerService
	{
		string SanititzeHtml(string input);
		T SanitizeDto<T>(T inputDto);
	}
}
