# Symulator ruchu drogowego

Program generujący w sposób losowy przestrzeń miejską i uzupełniający ją pojazdami oraz pieszymi koegzystującymi w ruchu drogowym.
  
# Ustawienia symulacji

Pierwszym ekranem jest generator symulacji, który udostępnia wybór:
* szerokości mapy
* wysokości mapy
* liczby wejść na mapę
* liczby pieszych
* liczby samochodów

W przypadku chęci wygenerowania symulacji wystarczy wcisnąć przycisk "Generuj symulację". W przypadku istnienia jednej symulacji zostanie ona zakończona i zastąpiona.
  
# Generowanie poziomu

### Generowanie dróg
  
W pierwszej kolejności na podstawie szerokości, wysokości i ilości punktów wejścia generowanie są punkty wejścia umieszczone na krawędziach mapy z których to będą pojawiać się samochody. Następnie tworzone są połączenia między nimi a później redukowane są ślepe ścieżki. Tym sposobem tworzony jest graf drogi, którego spójność jest weryfikowana na podstawie algorytmu DFS.

Tworzone są takie pola jak:
* Drogi
* Skrzyżowania
* Zakręty
* Przejścia dla pieszych
* Punkty wejścia

### Generowanie znaków pionowych

Na podstawie wygenerowanej drogi nakreślane na drodze są znaki pionowe.

* Pasy w miejscach przejść dla pieszych
* linia przerywana w miejscach dróg
* Linia podwójna ciągła na zakrętach
  
### Generowanie przestrzeni

Na podstawie wygenerowanego grafu tworzona jest dwuwymiarowa tablica zawierająca informacje o pustych polach. Z tablicy zbierane są prostokąty pustych pól, na ich podstawie generowane są przejścia dla pieszych.

Ponownie wyznaczane są prostokąty pustych pól i generowane są budynki w przestrzeni miejskiej. W zależności od wielkości prostokątów generowane są różne kombinacje budynków z przygotowanych wcześniej elementów.

W polach które były zbyt małe na pomieszczenie budynku generowane są ozdoby w postaci drzew i obiektów miejskich.

### Generowanie chodników

Z generatora przestrzeni pobierane są chodniki. Na podstawie grafu drogi generowane są chodniki po obu stronach drogi. W miejscach przejść dla pieszych generowane są połączenia między dwoma stronami chodnika. Tym sposobem tworzony jest graf drogi, którego spójność jest weryfikowana na podstawie algorytmu DFS.

# Kontroler ruchu

Do utrzymywania zmiennej liczby samochodów i pieszych wykorzystywana jest metoda która co określony czas decyduje czy powinien pojawić się kolejny obiekt. 1/2 maksymalnej liczby jest minimalną liczbą obiektów a wartość pomiędzy oscyluje pomiędzy 0% a 100%.

W celu zapewnienia poruszania się pieszych i samochodów wykorzystuje się metodę która co określony czas uruchamia metodę odpowiedzialną za logikę poruszania się obiektów.

Każdy pojawiający się obiekt na mapie ma wylosowany punkt wejścia i punkt wyjścia oraz trasę pomiędzy dwoma punktami która jest wyznaczana za pomocą algorytmu Dijkstry.

### Kontroler samochodów

Samochód porusza się po grafie drogi, sprawdza czy może wejść na następne pole, jeśli pole zezwoli na wejście wychodzi z obecnego pola i przechodzi na następne udostępniając tym miejsce pojazdowi za nim. Rozwiązanie to ma na celu wyeliminowanie nachodzenia na siebie dwóch pojazdów. 

Na skrzyżowanie dostęp ma ten samochód który wjedzie jako pierwszy. W przypadku kiedy drugi samochód nie przecina drogi obecnego już na skrzyżowaniu samochodu może wjechać i on.

### Kontroler pieszych

Pieszy porusza się po grafie chodnika, sprawdza czy może wejść na następny wierzchołek grafu, jeśli jest to wierzchołek przejścia dla pieszych weryfikuje czy przejście nie jest zajmowane przez samochód i przechodzi przez nie.

# Uwagi

W celu upłynnienia ruchu drogowego, pojazdy nie zawsze poruszają się z zasadami pierwszeństwa ruchu drogowego.

Bywa iż pojazdy na skrzyżowaniach nachodzą na siebie.

W przypadku nadmiernego ruchu drogowego pojazdy na skrzyżowaniach mogą się zablokować powodując permanentny korek.

# Informacje

Program został napisany w C# z użyciem technologi WPF przez TukanHan.

W programie użyto obrazki pieszych należących do Kenney.

W programie użyto obrazki pojazdów należących do 1001.