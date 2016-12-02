using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using GoodBytes.Infrastructure.Utils.Interfaces;

namespace GoodBytes.Infrastructure.Utils.Services
{
	public enum Dimensions
	{
		Width,
		Height
	}

	public enum AnchorPosition
	{
		Top,
		Center,
		Bottom,
		Left,
		Right
	}

	public class ImageService : IImagesTreatment
	{
		public bool Save(string name, Image pic)
		{
			ImageCodecInfo myImageCodecInfo = null;
			Encoder myEncoder = null;
			EncoderParameter myEncoderParameter = null;
			EncoderParameters myEncoderParameters = null;

			myImageCodecInfo = GetEncoderInfo("image/jpeg");

			// for the Quality parameter category.
			myEncoder = Encoder.Quality;
			myEncoderParameters = new EncoderParameters(1);

			// Save the bitmap as a JPEG file with quality level 100.
			myEncoderParameter = new EncoderParameter(myEncoder, 100L);
			myEncoderParameters.Param[0] = myEncoderParameter;
			//save new image to file system.
			if (File.Exists(name))
				File.Delete(name);
			pic.Save(name, myImageCodecInfo, myEncoderParameters);
			return true;
		}

		public bool SaveThumbnail(string name, Image pic)
		{
			if (File.Exists(name))
				File.Delete(name);
			pic.Save(name, ImageFormat.Png);
			return true;
		}

		public Image Crop(Image img, int x, int y, int w, int h)
		{
			var bmPhoto = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(72, 72);
			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
			grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
			grPhoto.DrawImage(img, new Rectangle(0, 0, w, h), x, y, w, h, GraphicsUnit.Pixel);
			return bmPhoto;
		}

		public Image Crop(Image imgPhoto, int width, int height, AnchorPosition anchor)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			const int sourceX = 0;
			const int sourceY = 0;
			int destX = 0;
			int destY = 0;
			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;
			nPercentW = (Convert.ToSingle(width) / Convert.ToSingle(sourceWidth));
			nPercentH = (Convert.ToSingle(height) / Convert.ToSingle(sourceHeight));
			if (nPercentH < nPercentW)
			{
				nPercent = nPercentW;
				switch (anchor)
				{
					case AnchorPosition.Top:
						destY = 0;
						break;
					// break
					case AnchorPosition.Bottom:
						destY = Convert.ToInt32((height - (sourceHeight * nPercent)));
						break;
					// break
					default:
						destY = Convert.ToInt32(((height - (sourceHeight * nPercent)) / 2));
						break;
						// break
				}
			}
			else
			{
				nPercent = nPercentH;
				switch (anchor)
				{
					case AnchorPosition.Left:
						destX = 0;
						break;
					// break
					case AnchorPosition.Right:
						destX = Convert.ToInt32((width - (sourceWidth * nPercent)));
						break;
					// break
					default:
						destX = Convert.ToInt32(((width - (sourceWidth * nPercent)) / 2));
						break;
						// break
				}
			}
			int destWidth = Convert.ToInt32((sourceWidth * nPercent));
			int destHeight = Convert.ToInt32((sourceHeight * nPercent));
			var bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
			grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
			grPhoto.Dispose();
			return bmPhoto;
		}

		public Image RotateLeft(Image img)
		{
			img.RotateFlip(RotateFlipType.Rotate270FlipNone);
			return img;
		}

		public Image RotateRight(Image img)
		{
			img.RotateFlip(RotateFlipType.Rotate90FlipNone);
			return img;
		}

		private ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			ImageCodecInfo[] encoders = null;
			int j = 0;
			encoders = ImageCodecInfo.GetImageEncoders();
			for (j = 0; j <= encoders.Length; j++)
				if (encoders[j].MimeType == mimeType)
					break;
			return encoders[j];
		}
	}

	public class ResizeService : IImagesTreatmentResize
	{
		public Image ScaleByPercent(Image imgPhoto, int percent)
		{
			float nPercent = (Convert.ToSingle(percent) / 100);
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			const int sourceX = 0;
			const int sourceY = 0;
			const int destX = 0;
			const int destY = 0;
			int destWidth = Convert.ToInt32((sourceWidth * nPercent));
			int destHeight = Convert.ToInt32((sourceHeight * nPercent));
			var bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
			grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
			grPhoto.Dispose();
			return bmPhoto;
		}

		public Image ConstrainProportions(Image imgPhoto, int size, Dimensions dimension)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			const int sourceX = 0;
			const int sourceY = 0;
			const int destX = 0;
			const int destY = 0;
			float nPercent = 0;
			switch (dimension)
			{
				case Dimensions.Width:
					nPercent = (Convert.ToSingle(size) / Convert.ToSingle(sourceWidth));
					break;
				default:
					nPercent = (Convert.ToSingle(size) / Convert.ToSingle(sourceHeight));
					break;
			}
			int destWidth = Convert.ToInt32(sourceWidth * nPercent);
			int destHeight = Convert.ToInt32(sourceHeight * nPercent);
			var bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
			grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
			grPhoto.Dispose();
			return bmPhoto;
		}

		public Image FixedSize(Image imgPhoto, int width, int height, Color color)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			const int sourceX = 0;
			const int sourceY = 0;
			int destX = 0;
			int destY = 0;
			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;
			nPercentW = (Convert.ToSingle(width) / Convert.ToSingle(sourceWidth));
			nPercentH = (Convert.ToSingle(height) / Convert.ToSingle(sourceHeight));
			if (nPercentH < nPercentW)
			{
				nPercent = nPercentH;
				destX = Convert.ToInt32(((width - (sourceWidth * nPercent)) / 2));
			}
			else
			{
				nPercent = nPercentW;
				destY = Convert.ToInt32(((height - (sourceHeight * nPercent)) / 2));
			}
			int destWidth = Convert.ToInt32(sourceWidth * nPercent);
			int destHeight = Convert.ToInt32(sourceHeight * nPercent);
			var bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
			bmPhoto.MakeTransparent();
			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.Clear(color);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
			grPhoto.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
			grPhoto.Dispose();
			return bmPhoto;
		}

		public Image ResizeImageFixedWidth(Image imgToResize, int width)
		{
			int sourceWidth = imgToResize.Width;
			int sourceHeight = imgToResize.Height;

			float nPercent = (width / (float)sourceWidth);

			var destWidth = (int)(sourceWidth * nPercent);
			var destHeight = (int)(sourceHeight * nPercent);

			var b = new Bitmap(destWidth, destHeight);
			Graphics g = Graphics.FromImage(b);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
			g.Dispose();

			return b;
		}

		public Image ScaleImageWithMax(Image image, int maxWidth, int maxHeight)
		{
			double ratioX = (double)maxWidth / image.Width;
			double ratioY = (double)maxHeight / image.Height;

			double ratio = Math.Min(ratioY, ratioX);

			var newWidth = (int)(image.Width * ratio);
			var newHeight = (int)(image.Height * ratio);

			var newImage = new Bitmap(newWidth, newHeight);

			Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

			return newImage;
		}

		public Image GetThumbNail(Image source, int width, int height)
		{
			Size newSize = ResizeKeepAspect(source.Size, width, height);
			using (Image thumbnail = new Bitmap(width, height, PixelFormat.Format32bppRgb))
			{
				using (Graphics g = Graphics.FromImage(thumbnail))
				{
					g.Clear(Color.Transparent);
					double ratioW = (double)width / (double)source.Width;
					double ratioH = (double)height / (double)source.Height;
					double ratio = ratioW < ratioH ? ratioW : ratioH;
					int insideWidth = (int)(source.Width * ratio);
					int insideHeight = (int)(source.Height * ratio);

					g.DrawImage(source, new Rectangle((width / 2) - (insideWidth / 2), (height / 2) - (insideHeight / 2), insideWidth, insideHeight));
				}
				var b = new Bitmap(thumbnail);
				b.MakeTransparent();
				return b;
			}
		}

		private static Size ResizeKeepAspect(Size src, int maxWidth, int maxHeight)
		{
			decimal rnd = Math.Min(maxWidth / (decimal)src.Width, maxHeight / (decimal)src.Height);
			return new Size((int)Math.Round(src.Width * rnd), (int)Math.Round(src.Height * rnd));
		}
	}

	public class WaterMarkService : IImagesTreatmentWaterMark
	{
		public Image AddWaterMark(Image ig, Image wm, int posX, int posY, string phrase)
		{
			int phWidth = ig.Width;
			int phHeight = ig.Height;
			var bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(ig.HorizontalResolution, ig.VerticalResolution);
			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			int wmWidth = wm.Width;
			int wmHeight = wm.Height;
			grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
			grPhoto.DrawImage(ig, new Rectangle(0, 0, phWidth, phHeight), 0, 0, phWidth, phHeight, GraphicsUnit.Pixel);

			int[] sizes =
			{
				20,
				18,
				16,
				14,
				12,
				10,
				8
			};
			Font crFont = null;
			var crSize = new SizeF();
			int i = 0;
			while (i < 7)
			{
				crFont = new Font("arial", sizes[i], FontStyle.Bold);
				crSize = grPhoto.MeasureString(phrase, crFont);
				Math.Min(Interlocked.Increment(ref i), i - 1);
			}
			int yPixlesFromBottom = Convert.ToInt32((phHeight * 0.05));
			float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));
			float xCenterOfImg = (phWidth / 2);
			var strFormat = new StringFormat();
			strFormat.Alignment = StringAlignment.Center;
			var semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
			grPhoto.DrawString(phrase, crFont, semiTransBrush2, new PointF(xCenterOfImg + 1, yPosFromBottom + 1), strFormat);
			var semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
			grPhoto.DrawString(phrase, crFont, semiTransBrush, new PointF(xCenterOfImg, yPosFromBottom), strFormat);

			var bmWatermark = new Bitmap(bmPhoto);
			bmWatermark.SetResolution(ig.HorizontalResolution, ig.VerticalResolution);
			Graphics grWatermark = Graphics.FromImage(bmWatermark);
			var imageAttributes = new ImageAttributes();
			var colorMap = new ColorMap();
			colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
			colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
			ColorMap[] remapTable = { colorMap };
			imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
			float[][] colorMatrixElements =
			{
				new[]
				{
					1f,
					0f,
					0f,
					0f,
					0f
				},
				new[]
				{
					0f,
					1f,
					0f,
					0f,
					0f
				},
				new[]
				{
					0f,
					0f,
					1f,
					0f,
					0f
				},
				new[]
				{
					0f,
					0f,
					0f,
					0.3f,
					0f
				},
				new[]
				{
					0f,
					0f,
					0f,
					0f,
					1f
				}
			};
			var wmColorMatrix = new ColorMatrix(colorMatrixElements);
			imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

			//Dim xPosOfWm As Integer = ((phWidth - wmWidth) - Pos)
			//Dim yPosOfWm As Integer = Pos
			int xPosOfWm = posX;
			int yPosOfWm = posY;

			grWatermark.DrawImage(wm, new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight), 0, 0, wmWidth, wmHeight,
				GraphicsUnit.Pixel, imageAttributes);
			ig = bmWatermark;
			grPhoto.Dispose();
			grWatermark.Dispose();
			return ig;
			//imgPhoto.Dispose()
		}
	}
}