using System.Drawing;
using GoodBytes.Infrastructure.Utils.Services;

namespace GoodBytes.Infrastructure.Utils.Interfaces
{
	public interface IImagesTreatment
	{
		bool Save(string name, Image pic);
		bool SaveThumbnail(string name, Image pic);
		Image Crop(Image img, int x, int y, int w, int h);
		Image Crop(Image imgPhoto, int width, int height, AnchorPosition anchor);
		Image RotateLeft(Image img);
		Image RotateRight(Image img);
	}

	public interface IImagesTreatmentResize
	{
		Image ScaleByPercent(Image imgPhoto, int percent);
		Image ConstrainProportions(Image imgPhoto, int size, Dimensions dimension);
		Image FixedSize(Image imgPhoto, int width, int height, Color color);
		Image ResizeImageFixedWidth(Image imgToResize, int width);
		Image ScaleImageWithMax(Image image, int maxWidth, int maxHeight);
		Image GetThumbNail(Image source, int width, int height);
	}

	public interface IImagesTreatmentWaterMark
	{
		Image AddWaterMark(Image ig, Image wm, int posX, int posY, string phrase);
	}
}