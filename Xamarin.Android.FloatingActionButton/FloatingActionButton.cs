using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidRes = Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Annotation;

namespace Android.Widget.Extras
{
	public class FloatingActionButton : View
	{
		private const float DEFAULT_RADIUS  = 25;

		private float    _radius;
		private float    _contentScale = 3.0f / 2;
		private Context  _context;
		private Color    _backgroundColor;
		private Color    _shadowColor;
		private Drawable _drawableContent = null;
		private Paint    _buttonBackgroundPaint;
		private Paint    _shadowPaint;

		public float RadiusInDp { get { return _radius; } set { _radius = value; PostInvalidate(); } }

		public Color BackgroundColor { get { return _backgroundColor; } set { _backgroundColor = value; PostInvalidate(); } }

		public Color ShadowColor { get { return _shadowColor; } set { _shadowColor = value; PostInvalidate(); } }

		public Drawable DrawableContent { get { return _drawableContent; } set { _drawableContent = value; PostInvalidate(); } }

		public FloatingActionButton(Context context) : base(context)
		{
			_context = context;
			Init();
		}

		public FloatingActionButton(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			_context = context;
			ObtainStyles(context, attrs, 0, 0);
			Init();
		}

		public FloatingActionButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			_context = context;
			ObtainStyles(context, attrs, defStyleAttr, 0);
			Init();
		}
		
		[TargetApi(Value = 21)]
		public FloatingActionButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			_context = context;
			ObtainStyles(context, attrs, defStyleAttr, defStyleRes);
			Init();
		}

		protected FloatingActionButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer){ }

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);
			_buttonBackgroundPaint.Color = _backgroundColor;

			float diameter = _radius * 2;
			float uWidth = PaddingLeft == 0 && PaddingRight == 0 ? diameter : Width - PaddingLeft - PaddingRight;
			float uHeight = PaddingBottom == 0 && PaddingTop == 0 ? diameter : Height - PaddingTop - PaddingBottom;
			float cx = uWidth / 2;
			float cy = uHeight / 2;
			float radius = Math.Min(uWidth - DipToPixels(2), uHeight - DipToPixels(2)) / 2;

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
			{
				SetLayerType(LayerType.Software, null);
			}
            _shadowPaint.SetShadowLayer(10, 0, 0, _shadowColor);
			canvas.DrawCircle(cx, cy, radius - 4, _shadowPaint);
			canvas.DrawCircle(cx, cy, radius, _buttonBackgroundPaint);
			
			int sidePadding = (int)(_radius * 0.25);
			int ulPadding = (int)(_radius * (0.25 + _contentScale));
			_drawableContent?.SetBounds(sidePadding, sidePadding, ulPadding, ulPadding);
			_drawableContent?.Draw(canvas);
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
			int diameterPadded = (int)(2 * _radius);
			int w = ResolveSize(diameterPadded + PaddingLeft + PaddingRight, widthMeasureSpec);

			int h = ResolveSize(diameterPadded + PaddingTop + PaddingBottom, heightMeasureSpec);
			SetMeasuredDimension(w, h);
		}

		private void ObtainStyles(Context con, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
		{
			TypedArray tArray = con.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.FloatingActionButton, defStyleAttr, defStyleRes);
			try
			{
                _radius = tArray.GetDimension(Resource.Styleable.FloatingActionButton_fab_radius, DipToPixels(DEFAULT_RADIUS));
				_backgroundColor = tArray.GetColor(Resource.Styleable.FloatingActionButton_fab_backgroundColor, Android.Resource.Color.BackgroundDark);
				_drawableContent = tArray.GetDrawable(Resource.Styleable.FloatingActionButton_fab_content);
				_shadowColor = tArray.GetColor(Resource.Styleable.FloatingActionButton_fab_shadowColor, Resource.Color.shadow_color);
			}
			finally
			{
				tArray.Recycle();
			}
		}

		private void Init()
		{
			_buttonBackgroundPaint = new Paint(PaintFlags.AntiAlias);
			_buttonBackgroundPaint.SetMaskFilter(new BlurMaskFilter(2, BlurMaskFilter.Blur.Normal));
			_shadowPaint = new Paint();
			_shadowPaint.AntiAlias = true;
        }

		private int DipToPixels(int dip)
		{
			return (int)DipToPixels((float)dip);
		}

		private float DipToPixels(float dip)
		{
			return dip * Resources.DisplayMetrics.Density + 0.5f;
        }
	}
}