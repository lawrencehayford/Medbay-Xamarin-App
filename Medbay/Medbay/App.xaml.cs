using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;

using Xamarin.Forms;

namespace Medbay
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
           AppCenter.Start("e97a62ab-7f0a-4fef-99e2-d3c5d74a2db5", typeof(Push));
             
            MainPage = new Medbay.MainPage();
		}


		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
