﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SpecNuts.Json
{
	public class ReportContractResolver : DefaultContractResolver
	{
		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			var properties = base.CreateProperties(type, memberSerialization);

			foreach (var property in properties)
			{
				property.PropertyName = ConvertPropertyName(property.PropertyName);
			}

			// only seria
			return properties;
		}

		private string ConvertPropertyName(string name)
		{
			var result = new StringBuilder();

			for (var i = 0; i < name.Length; i++)
			{
				var c = name[i];
				if (Char.IsUpper(c))
				{
					if (i > 0)
					{
						result.Append('_');
					}
					result.Append(c.ToString().ToLower());
				}
				else
				{
					result.Append(c);
				}
			}
			return result.ToString();
		}
	}
}