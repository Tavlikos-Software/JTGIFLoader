//JTGIFLoader - a library for animating GIF files in MonoTouch
//© 2012, Dimitris Tavlikos - dimitris ( at ) tavlikos.com, http://software.tavlikos.com
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.ImageIO;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace JTGIFLoader
{
	public class GIFLoader
	{

		public const string GIFDelayTimeKey = "DelayTime";
		public const string GIFKey = "{GIF}";

		public static UIImage LoadGIFImage(string gifImage)
		{

			try
			{

				using (NSData imgData = NSData.FromFile(gifImage))
				using (CGImageSource imgSource = CGImageSource.FromData(imgData))
				{

					NSString gifKey = new NSString(GIFKey);
					NSString gifDelayKey = new NSString(GIFDelayTimeKey);
					List<double> frameDelays = new List<double>();

					// Array that will hold each GIF frame.
					UIImage[] frames = new UIImage[imgSource.ImageCount];

					// Get all individual frames and their delay time.
					for (int i = 0; i < imgSource.ImageCount; i++)
					{

						// Get the frame's property dictionary and the '{GIF}' dictionary from that.
						using (NSDictionary frameProps = imgSource.CopyProperties(null, i))
						using (NSMutableDictionary gifDict = (NSMutableDictionary)frameProps[gifKey])
						{

							double frameDelay = ((NSNumber)gifDict[gifDelayKey]).DoubleValue;
							frameDelays.Add(frameDelay);

						}//end using

						// Fill the array.
						using (CGImage cgImage = imgSource.CreateImage(i, null))
						{

							frames[i] = UIImage.FromImage(cgImage);

						}//end using cgImage

					}//end for

					// Create animated image.
					return UIImage.CreateAnimatedImage(frames, frameDelays.Sum());

				}//end using

			} catch (Exception ex)
			{
				// Something went wrong!
				throw ex;
			}//end try catch

		}//end static UIImage LoadGifImage
	}
}

