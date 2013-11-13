using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace net.jsBeautifier
{
	public partial class JsBeautifier : Form
	{
		#region Private Members
		private HtmlElement indentEl;
		private HtmlElement bracesEl;
		private HtmlElement emptyLinesEl;
		private HtmlElement detectPackEl;
		private HtmlElement arrayIndentEl;

		private HtmlElement jsEl;
		private HtmlElement beautifyBtnEl;

		private Uri htmlUri=new Uri(ResourceExtractor.resourceBaseDir + "\\index.html");
		private bool isInitialized=false;
		//private bool background=false;
		#endregion

		#region Constructor
		public JsBeautifier ()
		{
			InitializeComponent();
		}

		//public JsBeautifier (bool background)
		//{
		//    InitializeComponent();

		//    this.background = background;
		//}
		#endregion

		#region Public Events
		public event EventHandler BeautifierReady;
		#endregion

		#region Event Handlers
		private void JsBeautifier_Shown (object sender, EventArgs e)
		{
			this.initialize();
		}

		private void webBrowser1_DocumentCompleted (object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			if (e.Url.Equals(this.htmlUri))
			{
				this.initializeElements();
				this.isInitialized = true;

				if (this.BeautifierReady != null)
				{
					this.BeautifierReady(this, EventArgs.Empty);
				}
			}
		}

		#endregion

		#region Public Methods
		public void initialize ()
		{
			this.webBrowser1.Navigate(this.htmlUri);
		}

		public void setIndentation (int indent)
		{
			this.ensureInitialized();

			this.indentEl.SetAttribute("value", indent.ToString());
		}

		public void setBracesNewLine (bool check)
		{
			this.ensureInitialized();

			this.setCheckValue(this.bracesEl, check);
		}

		public void setPreserveEmptyLines (bool check)
		{
			this.ensureInitialized();

			this.setCheckValue(this.emptyLinesEl, check);
		}

		public void setDetectPackers (bool check)
		{
			this.ensureInitialized();

			this.setCheckValue(this.detectPackEl, check);
		}

		public void setKeepArrayIndent (bool check)
		{
			this.ensureInitialized();

			this.setCheckValue(this.arrayIndentEl, check);
		}

		public string getBeautifiedScript (string script)
		{
			this.ensureInitialized();

			this.jsEl.SetAttribute("value", script);
			this.webBrowser1.Document.InvokeScript("do_js_beautify");
			return (this.jsEl.GetAttribute("value"));
		}
		#endregion

		#region Private Properties
		private HtmlElement form
		{
			get
			{
				return (this.webBrowser1.Document.Forms[0]);
			}
		}
		#endregion

		#region Private Methods
		private void initializeElements ()
		{
			HtmlElement form=this.form;

			foreach (HtmlElement e in form.GetElementsByTagName("select"))
			{
				if (e.GetAttribute("id").Equals("tabsize"))
				{
					this.indentEl = e;
					break;
				}
			}

			foreach (HtmlElement e in form.GetElementsByTagName("input"))
			{
				switch (e.GetAttribute("id"))
				{
					case "braces-on-own-line":
						this.bracesEl = e;
						break;
					case "preserve-newlines":
						this.emptyLinesEl = e;
						break;
					case "detect-packers":
						this.detectPackEl = e;
						break;
					case "keep-array-indentation":
						this.arrayIndentEl = e;
						break;
				}
			}

			foreach (HtmlElement e in form.GetElementsByTagName("textarea"))
			{
				if (e.GetAttribute("id").Equals("content"))
				{
					this.jsEl = e;
					break;
				}
			}

			foreach (HtmlElement e in form.GetElementsByTagName("button"))
			{
				if (e.GetAttribute("id").Equals("beautify"))
				{
					this.beautifyBtnEl = e;
					break;
				}
			}
		}

		private void ensureInitialized ()
		{
			if (!this.isInitialized)
			{
				throw new InvalidOperationException("jsBeautifier is not initialized yet.");
			}
		}

		private void setCheckValue (HtmlElement el, bool check)
		{
			if (check)
				el.SetAttribute("checked", "checked");
			else
				el.SetAttribute("checked", "");
		}
		#endregion

		//#region Overrides
		//protected override CreateParams CreateParams
		//{
		//    get
		//    {
		//        if (this.background)
		//        {
		//            CreateParams cp=base.CreateParams;
		//            cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
		//            return cp;
		//        }
		//        else
		//        {
		//            return (base.CreateParams);
		//        }
		//    }
		//}

		//protected void InvalidateEx ()
		//{
		//    if (Parent == null)
		//        return;

		//    Rectangle rc=new Rectangle(this.Location, this.Size);
		//    Parent.Invalidate(rc, true);
		//}

		//protected override void OnPaintBackground (PaintEventArgs pevent)
		//{
		//    if (this.background)
		//    {
		//        //do not allow the background to be painted 
		//    }
		//    else
		//    {
		//        base.OnPaintBackground(pevent);
		//    }
		//}		
		//#endregion
	}
}
