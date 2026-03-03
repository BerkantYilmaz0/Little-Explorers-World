# Çoklu Mini Oyun (Minigame Hub) Dönüşümü

## Hedef
Projeyi tekil bir hayvancılık oyunundan, ana menüden farklı "bilmece" oyunlarına (Hayvanlar, Meyveler vb.) girilebilen esnek bir "Mini Oyun Merkezi" (Minigame Hub) mimarisine geçirmek.

## Görevler
- [x] Görev 1: Sahne yapısını düzenle -> Doğrula: `MainMenu`'den "Oyna" butonuna basınca `GameSelection` (Oyun Seçimi) sahnesine geçildiğini gör.
- [x] Görev 2: Oyun Seçim Ekranını (GameSelection) kur -> Doğrula: Ekranda "Hayvanları Bil" ve "Meyveleri Bil" gibi butonlar görünür olsun.
- [x] Görev 3: Mevcut oyunu izole et -> Doğrula: Mevcut `GamePlay` sahnesinin adını `AnimalQuizLevel` olarak değiştir ve "Hayvanları Bil" butonuna basınca açıldığını doğrula.
- [ ] Görev 4: Meyve oyunu için Data altyapısını kur -> Doğrula: `FruitData` ScriptableObject sınıfını oluştur ve proje içinde Asset olarak oluşturulabildiğini gör.
- [ ] Görev 5: Meyve oyunu sahnesini oluştur -> Doğrula: `FruitQuizLevel` sahnesini Editor aracı (örn. `FruitQuizSetup.cs`) ile kur ve `FruitQuizController`'ın çalıştığını ses+kart mantığıyla test et.
- [ ] Görev 6: Ödül Sistemini merkeze bağla -> Doğrula: Mini oyun bitince `LevelSelection` ya da `GameSelection` ekranına dönme butonunun çalıştığını gör.

## Bittiğinde
- [ ] Ana menüden "Oyna" dendiğinde Oyun Seçme ekranı gelmeli.
- [ ] Oyun Seçme ekranından "Hayvanları Bil" seçildiğinde mevcut çalıştığımız oyun açılmalı.
- [ ] Oyun Seçme ekranından "Meyveleri Bil" seçilince yeni meyve oyunu açılmalı.
- [ ] Her mini oyun bittiğinde (5 soru sonunda) başarıyla "Oyun Seçimi" ekranına geri dönülebilmeli.
- [ ] Yeni eklenecek mini oyunlar için (örn. Araçlar, Sayılar) altyapı modüler hale gelmiş olmalı.
