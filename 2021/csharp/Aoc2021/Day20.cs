namespace Aoc2021;

public class Day20 : IDay
{
    public override string Day { get; } = nameof(Day20)[3..];

    public override string Result1()
    {
        return ImageProcessor.Parse(InputLines)
            .Enhancement(2)
            .Pixels.Count
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string Result2()
    {
        return ImageProcessor.Parse(InputLines)
            .Enhancement(50)
            .Pixels.Count
            .ToString(CultureInfo.InvariantCulture);
    }

    public record ImageProcessor(HashSet<(int x, int y)> Pixels, int[] Algorithm)
    {
        public bool IsInverted { get; private set; }

        private bool IsInversionRequired => Algorithm[0] == 1;
        private int WhichPixelsToAdd => IsInversionRequired && !IsInverted ? 0 : 1;

        public ImageProcessor Enhancement(int count)
        {
            return Enhancements().ElementAt(count);
        }

        public IEnumerable<ImageProcessor> Enhancements()
        {
            ImageProcessor image = this;

            while (true)
            {
                yield return image;
                image = image.Enhanced();
            }
        }

        public ImageProcessor Enhanced()
        {
            int litValue = IsInverted ? 0 : 1;
            int dimValue = 1 - litValue;
            HashSet<(int, int)> processedPixelsCache = new();

            return this with
            {
                Pixels = Pixels.Aggregate(new HashSet<(int x, int y)>(), enhance_nearby_pixels),
                IsInverted = IsInversionRequired && !IsInverted,
            };

            HashSet<(int x, int y)> enhance_nearby_pixels(HashSet<(int x, int y)> pixels, (int, int) xy)
            {
                return dots(xy).Aggregate(pixels, enhance_pixel);
            }

            HashSet<(int x, int y)> enhance_pixel(HashSet<(int x, int y)> pixels, (int, int) xy)
            {
                if (!processedPixelsCache.Add(xy))
                {
                    return pixels;
                }

                int pixelCode = pixel_code(xy, Pixels);
                if (Algorithm[pixelCode] == WhichPixelsToAdd)
                {
                    _ = pixels.Add(xy);
                }
                return pixels;
            }

            int pixel_code((int x, int y) dot, HashSet<(int x, int y)> pixels)
            {
                return dots(dot)
                    .Aggregate(0, (code, pixel) =>
                    {
                        return (code * 2) + (pixels.Contains(pixel) ? litValue : dimValue);
                    });
            }

            (int x, int y)[] dots((int x, int y) dot)
            {
                (int x, int y) = dot;
                return new (int x, int y)[]
                    {
                    (x-1, y-1),
                    (x,   y-1),
                    (x+1, y-1),
                    (x-1, y  ),
                    (x,   y  ),
                    (x+1, y  ),
                    (x-1, y+1),
                    (x,   y+1),
                    (x+1, y+1),
                    };
            }
        }

        public static ImageProcessor Parse(IEnumerable<string> lines)
        {
            return new(parse_image(lines.Skip(2)), parse_algorithm(lines.First()));

            static int[] parse_algorithm(string line)
            {
                return line.Select(c => c switch
                    {
                        '#' => 1,
                        '.' => 0,
                        _ => throw new ArgumentException($"Invalid char '{c}' in algorithm", nameof(line)),
                    })
                    .ToArray();
            }

            static HashSet<(int x, int y)> parse_image(IEnumerable<string> lines)
            {
                return lines
                    .Select((line, index) => (data: line, index))
                    .Aggregate(new HashSet<(int x, int y)>(), add_row);

                static HashSet<(int x, int y)> add_row(HashSet<(int x, int y)> pixels, (string data, int index) line)
                {
                    return line.data
                        .Select((symbol, index) => (symbol, xy: (index, line.index)))
                        .Aggregate(pixels, add_pixel);
                }

                static HashSet<(int x, int y)> add_pixel(HashSet<(int x, int y)> pixels, (char symbol, (int, int) xy) pixel)
                {
                    return pixel.symbol switch
                    {
                        '#' => add(pixel.xy),
                        '.' => pixels,
                        _ => throw new ArgumentException($"Invalid symbol '{pixel.symbol}' in image data", nameof(pixel)),
                    };

                    HashSet<(int x, int y)> add((int, int) xy)
                    {
                        _ = pixels.Add(xy);
                        return pixels;
                    }
                }
            }
        }
    }
}
