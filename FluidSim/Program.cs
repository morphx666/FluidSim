using System;
using System.IO;

// This is a direct port of a fluid simulation program developed by Yusuke Endoh
// https://github.com/LeoColomb/FluidASCII/blob/master/endoh1_deobfuscate.c

// http://madebyevan.com/obscure-cpp-features/

namespace LiquidSim {
    public class Program {
        private const double G = 1.0; // Gravity factor
        private const double P = 4.0; // Pressure
        private const double V = 8.0; // Viscosity

        public static void Main(string[] args) {
            ComplexDouble[] a = new ComplexDouble[97687];
            ComplexDouble w, d;
            int p, q, r;
            int x, y;
            char[] b = new char[6856];
            int t;

            const string prefix = " '`-.|//,\\|\\_\\/#\n";

            Console.CursorVisible = false;
            int fileIndex = 0;
            FileInfo[] files = (new DirectoryInfo(@"..\..\FluidASCII")).GetFiles("*.txt");
            while(true) {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Project: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{files[fileIndex].Name.Split('.')[0].ToUpper()}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Press [ESC] to terminate, any other key to run next project");
                Console.ForegroundColor = ConsoleColor.White;
                string data = File.ReadAllText(files[fileIndex].FullName).Replace("\r", "");
                fileIndex = (fileIndex + 1) % files.Length;

                for(int i = 0; i < a.Length; i++) a[i] = new ComplexDouble();
                r = 0;
                w = 0;

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
                    for(x = 0; 2012 - 1 > x++;) {
                        b[x] = '\x0';
                    }
                    for(p = 0; p < r; p += 5) {
                        t = 10 + (x = (int)(a[p] * ComplexDouble.i).R) + 80 * (y = (int)(a[p] / 2).R);
                        a[p] += a[p + 4] += a[p + 3] / 10 * (a[p + 1].Power2() == 0 ? 1 : 0);
                        // x = 0 <= x && x < 79 && 0 <= y && y < 23 ? 1[1[*t |= 8, t] |= 4, t += 80] = 1, *t |= 2 : 0;
                        if(0 <= x && x < 79 && 0 <= y && y < 23) {
                            b[t] = (char)((byte)b[t] | 8); // *t |= 8
                            b[t + 1] = (char)((byte)b[t + 1] | 4); // 1[*t |= 8, t] |= 4
                            t += 80;
                            b[t + 1] = (char)1; // 1[1[*t |= 8, t] |= 4, t += 80] = 1
                            b[t] = (char)((byte)b[t] | 2); // *t |= 2
                        }
                    }
                    for(x = 0; 2012 - 1 > x++;) {
                        b[x] = prefix[(x % 80 - 9) != 0 ? b[x] : (prefix.Length - 1)];
                    }
                }

                if(Console.ReadKey().Key == ConsoleKey.Escape) return;
            }
        }
    }
}