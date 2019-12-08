using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Aoc._2019._08
{
    public class Run : BaseRun, IRun
    {
        public Run(string InputPrefix = "") : base(InputPrefix) { }

        public string Job1()
        {
            var image = Image.Parse(25, 6, ReadAllText("08/input1.txt"));
            return image.Checksum().ToString();
        }

        public string Job2()
        {
            var image = Image.Parse(25, 6, ReadAllText("08/input1.txt"));
            return image.Render();
        }
    }

    public class Image
    {
        public readonly Layer[] layers;

        public Image(Layer[] layers)
        {
            this.layers = layers;
        }

        public static Image Parse(int width, int height, string data)
        {
            var layers = (
                from layerPixels in LayerPixels()
                select Layer.Parse(width, height, layerPixels)
            ).ToArray();

            return new Image(layers);

            IEnumerable<string> LayerPixels()
            {
                int count = width * height;
                for (int i = 0; i < data.Length; i += count)
                {
                    yield return data[i..(i + count)];
                }
            }
        }

        public int Checksum()
        {
            var zeroCountQuery =
                from layer in layers
                select (count: layer.Count(0), layer);
            var minZeroLayer = (
                from zeroCount in zeroCountQuery
                orderby zeroCount.count
                select zeroCount.layer
            ).First();
            return minZeroLayer.Count(1) * minZeroLayer.Count(2);
        }

        public string Render()
        {
            int width = layers[0].width;
            int height = layers[0].height;
            var result = new StringBuilder();
            for (int y = 0; y < height; ++y)
            {
                result.Append('\n');
                for (int x = 0; x < width; ++x)
                {
                    var pixel = Compose(y * width + x);
                    var c = pixel switch
                    {
                        0 => ' ',
                        1 => '*',
                        _ => 'X',
                    };
                    result.Append(c);
                }
            }

            return result.ToString();

            int Compose(int i)
            {
                for (int layer = 0; layer < layers.Length; ++layer)
                {
                    int pixel = layers[layer].pixels[i];
                    if (pixel != 2)
                    {
                        return pixel;
                    }
                }
                return 2;
            }
        }
    }

    public class Layer
    {
        public readonly int width;
        public readonly int height;
        public readonly int[] pixels;

        Layer(int width, int height, int[] pixels)
        {
            this.width = width;
            this.height = height;
            this.pixels = pixels;
        }

        public static Layer Parse(int width, int height, string pixels)
        {
            var ints = (from pixel in pixels select pixel - '0').ToArray();
            return new Layer(width, height, ints);
        }

        public int Count(int i)
        {
            return (from pixel in pixels where pixel == i select i).Count();
        }
    }
}
