using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

// Token: 0x0200002B RID: 43
[CompilerGenerated]
[DebuggerNonUserCode]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
internal class Class34
{
	// Token: 0x06000128 RID: 296 RVA: 0x0020C800 File Offset: 0x0020AA00
	internal Class34()
	{
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000129 RID: 297 RVA: 0x0021FDBC File Offset: 0x0021DFBC
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager_0
	{
		get
		{
			if (Class34.resourceManager_0 == null)
			{
				ResourceManager resourceManager = new ResourceManager("i~@UWtP9Qn_'\\,K>Qe%I1)dR#%", typeof(Class34).Assembly);
				Class34.resourceManager_0 = resourceManager;
			}
			return Class34.resourceManager_0;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600012A RID: 298 RVA: 0x0021FDFC File Offset: 0x0021DFFC
	// (set) Token: 0x0600012B RID: 299 RVA: 0x0021FE10 File Offset: 0x0021E010
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo CultureInfo_0
	{
		get
		{
			return Class34.cultureInfo_0;
		}
		set
		{
			Class34.cultureInfo_0 = value;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600012C RID: 300 RVA: 0x0021FE24 File Offset: 0x0021E024
	internal static byte[] Byte_0
	{
		get
		{
			object @object = Class34.ResourceManager_0.GetObject("iboot", Class34.cultureInfo_0);
			return (byte[])@object;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600012D RID: 301 RVA: 0x0021FE50 File Offset: 0x0021E050
	internal static Bitmap Bitmap_0
	{
		get
		{
			object @object = Class34.ResourceManager_0.GetObject("IPhone_8_vector", Class34.cultureInfo_0);
			return (Bitmap)@object;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600012E RID: 302 RVA: 0x0021FE7C File Offset: 0x0021E07C
	internal static Bitmap Bitmap_1
	{
		get
		{
			object @object = Class34.ResourceManager_0.GetObject("IPhone_X_vector", Class34.cultureInfo_0);
			return (Bitmap)@object;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600012F RID: 303 RVA: 0x0021FEA8 File Offset: 0x0021E0A8
	internal static byte[] Byte_1
	{
		get
		{
			object @object = Class34.ResourceManager_0.GetObject("MDM", Class34.cultureInfo_0);
			return (byte[])@object;
		}
	}

	// Token: 0x0400018B RID: 395
	private static ResourceManager resourceManager_0;

	// Token: 0x0400018C RID: 396
	private static CultureInfo cultureInfo_0;
}
