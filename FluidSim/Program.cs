using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

// This is a direct port of a fluid simulation program developed by Yusuke Endoh
// https://github.com/LeoColomb/FluidASCII/blob/master/

// http://madebyevan.com/obscure-cpp-features/

namespace LiquidSim {
    public class Program {
        private const double G = 1.0;  // Gravity factor    | 1.0
        private const double P = 4.0;  // Pressure          | 4.0
        private const double V = 8.0;  // Viscosity         | 8.0
        private const double R = 12.0; // Sim. Resolution   | 10.0

        private const int cw = 80;
        private const int ch = 25;

        public static void Main(string[] args) {
            const string prefix = " '`-.|//,\\|\\_\\/#\n";

            try {
                Console.SetWindowSize(cw, ch + 5);
                Console.SetBufferSize(cw, ch + 5);
            } catch { }
            Console.CursorVisible = false;
            Console.Title = $"FluidSim";

            int fileIndex = 0;
            FileInfo[] files = (new DirectoryInfo(@"..\..\FluidASCII")).GetFiles("*.txt");

            while(true) {
                string projectName = files[fileIndex].Name.Split('.')[0].ToUpper();
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("FluidSim ");
                Console.ForegroundColor = ConsoleColor.White; Console.Write("Project: ");
                Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine(projectName);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Press [ESC] to terminate, any other key to run next project");
                Console.ForegroundColor = ConsoleColor.White;
                string data = File.ReadAllText(files[fileIndex].FullName).Replace("\r", "");
                fileIndex = (fileIndex + 1) % files.Length;

                int cb = cw * ch;
                ComplexDouble[] a = new ComplexDouble[97687];
                ComplexDouble w = 0, d;
                int p, q, r = 0;
                int x, y;
                char[] b = new char[cb];
                int t;

                for(int i = 0; i < a.Length; i++) a[i] = new ComplexDouble();

                for(int i = 0; i < data.Length; i++) {
                    x = data[i];
                    // w = x > 10 ? 32 < x ? 4[*r++ = w, r] = w + 1, *r = r[5] = x == 35, r += 9 : 0, w - I : (x = w + 2);
                    if(x > 10) {
                        if(32 < x) {
                            a[r++] = w; // *r++ = w
                            a[r + 4] = w + 1; // 4[*r++ = w, r] = w + 1
                            a[r] = a[r + 5] = (x == 35 ? 1 : 0); // *r = r[5] = x == 35
                            r += 9;
                        }
                        w -= ComplexDouble.i;
                    } else {
                        w = (int)(w.R + 2);
                    }
                }

                for(; !Console.KeyAvailable;) {
                    Console.SetCursorPosition(0, 3);
                    Console.Write(b);

                    for(p = 0; p < r; p += 5) {
                        a[p + 2] = a[p + 1] * 9;
                        for(q = 0; q < r; q += 5) {
                            w = (a[p] - a[q]).Magnitude() / 2 - 1;
                            if(0 < (int)(1 - w).R) a[p + 2] += w * w;
                        }
                    }
                    for(p = 0; p < r; p += 5) {
                        a[p + 3] = G;
                        for(q = 0; q < r; q += 5) {
                            w = (d = (a[p] - a[q])).Magnitude() / 2 - 1;
                            if(0 < (int)(1 - w).R) a[p + 3] += w * (d * (3 - a[p + 2] - a[q + 2]) * P + a[p + 4] * V - a[q + 4] * V) / a[p + 2];
                        }
                    }
                    Array.Clear(b, 0, b.Length);
                    for(p = 0; p < r; p += 5) {
                        t = 10 + (x = (int)(a[p] * ComplexDouble.i).R) + cw * (y = (int)(a[p] / 2).R);
                        a[p] += a[p + 4] += a[p + 3] / R * (a[p + 1].R == 0 ? 1 : 0);
                        // x = 0 <= x && x < 79 && 0 <= y && y < 23 ? 1[1[*t |= 8, t] |= 4, t += 80] = 1, *t |= 2 : 0;
                        if(0 <= x && x < (cw - 1) && 0 <= y && y < (ch - 2)) {
                            b[t] = (char)((byte)b[t] | 8); // *t |= 8
                            b[t + 1] = (char)((byte)b[t + 1] | 4); // 1[*t |= 8, t] |= 4
                            b[(t += cw) + 1] = (char)1; // 1[1[*t |= 8, t] |= 4, t += 80] = 1
                            b[t] = (char)((byte)b[t] | 2); // *t |= 2
                        }
                    }
                    for(x = 0; cb - 1 > x++;) {
                        b[x] = prefix[(x % cw - 9) != 0 ? b[x] : 16];
                    }
                }

                if(Console.ReadKey().Key == ConsoleKey.Escape) return;
            }
        }
    }
}