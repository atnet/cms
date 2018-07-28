﻿/**
 * Copyright (C) 2007-2015 Z3Q.NET,All rights reseved.
 * Get more infromation of this software,please visit site http://k3f.net/cms
 * 
 * name : SqlFormat.cs
 * author : newmin (new.min@msn.com)
 * date : 2012/12/01 23:00:00
 * description : 
 * history : 
 */

using System;
using T2.Cms.Conf;
using JR.DevFw.Data;

namespace com.plugin.sso.Core
{
	/// <summary>
	/// Description of OrmMapping.
	/// </summary>
	internal class SqlFormat:ISqlFormat
	{
		public string Format(string source,params string[] objs)
		{
			source=source.Replace("$PREFIX_",Settings.DB_PREFIX);
			if(objs.Length!=0){
				source=String.Format(source,objs);
			}
			return source;
		}
	}
}
