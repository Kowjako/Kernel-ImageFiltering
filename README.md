# Filtrowanie obrazów — Kernel i nie tylko 🌄
Program przedstawia realizację filtrów konwolucyjnych i nie tylko, przedstawiono dużo presetów oraz możliwość dodania rożnego typu szumów, również istnieje możliwość oprócz presetów zrobić swoją macierz konwolucji i zastosować na wybranym obrazku lub załadować macierz z pliku.  
***Wszystkie filtry zaimplementowane ręcznie bez użycia gotowych bibliotek***
# Praca z obrazkiem i pixelami
Jak wiadomo wszystkie operacje na obrazku przeprowadzane są na pixelach.  
Więc pokażę pracę z obrazkiem na przykładzie filtru *GrayScale*. Zasada implementacji filtru jest prosta, musimy wziąć składowe pixela (R,G,B) oraz znaleźć średnią i następnie stworzyć pixel gdzie R, G, B będą sobie równe i rownać się wartości średniej.
## Sposób 1 — Wbudowane metody C#  
Metody **GetPixel**, **SetPixel** pozwalają operować bezpośrednio na pixelach i dostawać z tych pixeli potrzebne nam kolory, więc implementacja filtru wyglądałaby następująco:  
```c#
for (int j = 0; j < startImageBMP.Height; j++)
{
    for (int i = 0; i < startImageBMP.Width; i++)
    {
         Color pixel = startImageBMP.GetPixel(i,j);
         byte avg = (pixel.R + pixel.G + pixel.B) / 3;
         Color newPixel = Color.FromArgb(255, avg, avg, avg);
         outputImageBMP.SetPixel(i, j, newPixel);
     }
}
```
## Sposób 2 — Samodzielny za pomocą operacji logicznych  
```c#
for (int j = 0; j < startImageBMP.Height; j++)
{
    for (int i = 0; i < startImageBMP.Width; i++)
    {
         UInt32 pixel = (UInt32)(startImageBMP.GetPixel(i, j).ToArgb());

         float R = (pixel & 0x00FF0000) >> 16;   
         float G = (pixel & 0x0000FF00) >> 8;
         float B = (pixel & 0x000000FF);

         R = G = B = (R + G + B) / 3.0f;

         UInt32 newPixel = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | (UInt32)B;
         outputImageBMP.SetPixel(i, j, System.Drawing.Color.FromArgb((int)newPixel));
    }
}
```
Co dzieje się tutaj: Jak wiadomo Pixel składa się z 3 kolorów R,G,B każdy z których może przyjmować wartości od 0-255 czyli 1 bajt, jak wiadomo 1 bajt to są dwa znaki HEX.  
Czyli np. kolor **Cyan(R = 72, G = 209, B = 204) —> w systemie HEX Cyan(R = 48, G = D1, B = CC)** i zawsze jest Alpha-kanał wpisujemy mu zawsze wartość **255(FF)**.  
Więc w sumie mamy 4 bajty (1 bajt - Alpha kanał, 1 bajt - R, 1 bajt - G, 1 bajt - B) czyli 32 bity, w pamięci komputera ten kolor wygląda następująco: **0xFF48D1CC**.  
Teraz jeżeli pobieramy używając **GetPixel(i,j).ToArgb()** to on zwróci nam 32-bitowego Int'a.
I teraz spróbujemy dla tego koloru przekształcić go w GrayScale:  
1. Pobieramy czerwony, żeby to zrobić mnożymy z maską 0x00FF0000 co nam wyzeruje wszystkie bity oprócz czerwonego, ale następnie trzeba przesunąć bo 0x00FF0000 != 0x000000FF więc dajemy przesunięcie na 16 bitów (1 HEX = 4 bity) w prawo.
2. Analogicznie pobieramy G i B  
3. Obliczamy średnią i wpisujemy ją do wartości R, G, B
4. Pozostaje tylko utworzyć nasz kolor w postaci HEX ze składowych, więc posługujemy się logicznym dodawaniem i trzeba niezapomnieć przesunąć na właściwą pozycję:

# Klasa Bindable2DArray
Ta klasa była napisana z tego powodu że XAML nie pozwala stworzyć **Binding** do tablicy dwuwymiarowej zaś dla jednowymiarowej pozwala, więc ta klasa podmienia indeks tablicy dwuwymiarowej na jakby to był indeks jednowymiarowej bo jeżeli w XAML'u dać  
```c#
Binding Path = Array[2,3]
```
To nie będzie działało bo rozpoznaje że są dwa indeksy, zaś jeżeli podamy np. **Array[2-3]** to będzie to rozpatrywał jako jednowymiarowa zaś za prawidłową konwersję odpowiada klasa **Bindable2DArray**. W tej klasie jest jeszcze jedna rzecz:
- Przeciążony operator generyczny **T[,]** który pozwoli castować naszą klasę przedstawiającą tablicę dwuwymiarową na tablicę dwuwymiarową typów prymitywnych, np:
```c#
int[,] array = new Bindable2DArray(5,5);
```
# Wyniki filtrów
- Box Blur    
![bb](https://user-images.githubusercontent.com/19534189/127917957-7ba69ea8-bf0c-4452-9520-20ab0a84dcf6.jpg)
- Black-White  
![bw](https://user-images.githubusercontent.com/19534189/127917963-9c888569-74c3-4118-949a-de4729328e36.jpg)
- Color accent  
![ca](https://user-images.githubusercontent.com/19534189/127917965-ba38c583-1c73-47c3-bd77-86b4c3fb0140.jpg)
- Channel segregation  
![cs](https://user-images.githubusercontent.com/19534189/127917968-352911b4-f068-47eb-af29-e0f3226a5b18.jpg)
- Edge detection  
![ed](https://user-images.githubusercontent.com/19534189/127917969-60040e5d-0ae8-4bbe-9b6b-e00f74ab8d8e.jpg)
- Embossing  
![emb](https://user-images.githubusercontent.com/19534189/127917970-bbabf0d4-e357-43d2-8738-4bd8de8bf245.jpg)
- Extension  
![ext](https://user-images.githubusercontent.com/19534189/127917972-4b8508c4-32fe-4c48-9347-e5ff9dc7c37b.jpg)
- Gaussian Blur 3x3  
![gb3](https://user-images.githubusercontent.com/19534189/127917975-b19d0978-0e25-443a-bb72-9929aa6a921b.jpg)
- Gaussian Blur 5x5  
![gb5](https://user-images.githubusercontent.com/19534189/127917978-9a054548-e115-472a-bf3b-25adbeb25b78.jpg)
- Glowing edges  
![ge](https://user-images.githubusercontent.com/19534189/127917980-bdc1598d-4464-47a0-9422-956890856db4.jpg)
- Grassfire  
![gf](https://user-images.githubusercontent.com/19534189/127917982-d77424b3-9018-4877-9f22-65b5ddd4bfc2.jpg)
- Glass  
![glass](https://user-images.githubusercontent.com/19534189/127917983-81ce5512-1c2a-4dd4-93af-ad791b8075f3.jpg)
- Grayscale  
![gs](https://user-images.githubusercontent.com/19534189/127917985-aa2ae32e-fa54-4cc3-b02d-f24fc9acb452.jpg)
- Kuwahara's Filter  
![kuw](https://user-images.githubusercontent.com/19534189/127917986-536839d0-9c02-4d6d-b1bf-b6e901eab6b2.jpg)
- Median 1px  
![med](https://user-images.githubusercontent.com/19534189/127917989-39ed3561-d641-40fc-8eaa-f2c96a1d945a.jpg)
- Mirror  
![mir](https://user-images.githubusercontent.com/19534189/127917994-f0cd926b-425f-4793-81f6-2e0b18efb7d0.jpg)
- Negative  
![neg](https://user-images.githubusercontent.com/19534189/127917997-4dfa664b-be97-456a-91c7-5268e2cad915.jpg)
- Pixelization  
![pixe](https://user-images.githubusercontent.com/19534189/127917998-edf0fb00-607c-40b0-8757-58e363b081a9.jpg)
- Posterize  
![post](https://user-images.githubusercontent.com/19534189/127918000-224566c2-b798-489e-af78-a4ab83ea386a.jpg)
- Prewitt's Filter  
![prew](https://user-images.githubusercontent.com/19534189/127918001-5dc72746-c796-46dc-acbb-81cf56f11d25.jpg)
- Sepia  
![sep](https://user-images.githubusercontent.com/19534189/127918005-e3f4a7be-ef66-4541-9be3-0de0995dbda7.jpg)
- Sharpen  
![shr](https://user-images.githubusercontent.com/19534189/127918008-3a226dbe-450a-486c-89c2-ef31a21ef4ab.jpg)
- Solar  
![solar](https://user-images.githubusercontent.com/19534189/127918009-808cfa67-1051-4e9b-a9a2-cb8722357f56.jpg)
- Sobel's Filter  
![son](https://user-images.githubusercontent.com/19534189/127918012-02aa5615-3c0b-4d09-8e17-f33af1a2ca97.jpg)
- Vignette  
![vign](https://user-images.githubusercontent.com/19534189/127918013-c0f3fd19-bc63-4d80-8a79-a313166e323b.jpg)
# Screenshot
![program](https://user-images.githubusercontent.com/19534189/127918622-fd7c68e6-5da1-4315-a89d-557adb1f9292.png)
