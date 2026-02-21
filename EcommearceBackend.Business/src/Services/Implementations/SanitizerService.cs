using EcommearceBackend.Business.src.Services.Abstractions;
using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommearceBackend.Business.src.Services.Implementations
{
	public class SanitizerService : ISanitizerService
	{
		private readonly HtmlSanitizer _htmlSanitizer;
		public SanitizerService(HtmlSanitizer htmlSanitizer)
		{
			_htmlSanitizer= htmlSanitizer;
		}
		public string SanititzeHtml(string input)
		{
			return _htmlSanitizer.Sanitize(input);
		}

		public T SanitizeDto<T>(T inputDto)
		{
			var properties = typeof(T).GetProperties();
			foreach(var property in properties)
			{
				 if(property.PropertyType == typeof(string) && property.Name.ToLower()!="password"&&property.Name.ToLower()!="imageurl")

				{
					var value =property.GetValue(inputDto);
					if(value!=null)
					{
						var sanitizedValue = _htmlSanitizer.Sanitize(value.ToString());
						property.SetValue(inputDto, sanitizedValue);
					}
				}
						
            }
			return inputDto;
		}
	}
}
