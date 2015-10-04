using System;
using Android.App;
using Android.Widget;
using Android.OS;
using FAB = Android.Widget.Extras;
using And = Android;
using System.Collections.Generic;

namespace Xamarin.Android.FloatingActionButton.Sample
{
	[Activity(Label = "FloatingActionButton.Sample", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/BaseAppTheme")]
	public class MainActivity : Activity
	{
		private FAB.FloatingActionButton fab;
		private ListView dummyList;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);
			fab = FindViewById<FAB.FloatingActionButton>(Resource.Id.fab);
			fab.Click += Fab_Click;

			dummyList = FindViewById<ListView>(Resource.Id.dummyList);
			SetDummyData(dummyList);
		}

		private void Fab_Click(object sender, EventArgs e)
		{
			Toast.MakeText(this, "Clicked!", ToastLength.Short).Show();
		}

		private void SetDummyData(ListView v)
		{
			List<string> adapterData = GetDummyData();
			IListAdapter adapter = new ArrayAdapter<string>(this, And.Resource.Layout.SimpleListItem1, adapterData);
			v.Adapter = adapter;
		}

		private List<string> GetDummyData()
		{
			List<string> data = new List<string>();
			for (int i = 0; i < 20; i++)
			{
				data.Add("Item " + i.ToString());
			}
			return data;
		}
	}
}

