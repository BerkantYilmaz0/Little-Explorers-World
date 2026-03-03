# Plan: Minik Kaşifler Dünyası

3-5 yaş arası okul öncesi çocuklar için tasarlanmış, temel kavramları (hayvanlar, renkler, şekiller) keşfetmeyi amaçlayan 2D eğitici Unity oyunu.

## Proje Türü
**OYUN (Mobil - Android/iOS)**

---

## Başarı Kriterleri
- Metin içermeyen, tamamen ikon ve ses tabanlı kullanıcı arayüzü (UX).
- Minimum 3 farklı kategori (Çiftlik Hayvanları, Renk Bahçesi, Şekil Eşleştirme).
- 60 FPS performans hedefi (Mobil cihazlarda akıcı animasyonlar).
- Tamamen dokunmatik (Drag & Drop, Tap) kontrol sistemi.

## Teknoloji Yığını
- **Motor:** Unity 2022.3 LTS
- **Render Pipeline:** Universal Render Pipeline (URP - 2D Renderer)
- **Dil:** C#
- **Varlık Türleri:** 2D Sprite, Skeletal Animation (Spine/Unity 2D Animation), High-Quality Audio.

## Dosya Yapısı
```
Assets/
├── _Project/
│   ├── Scenes/          # Splash, MainMenu, GamePlay
│   ├── Scripts/         # Managers, Interactions, Audio
│   ├── Prefabs/         # UI Elements, Collectibles
│   ├── Sprites/         # Characters, Backgrounds
│   ├── Audio/           # Voiceovers, SFX, BGM
│   └── Animations/      # Controller, Clips
```

---

## Görev Kırılımı

### Faz 1: Temel Kurulum (Foundation)
- [ ] **Görev 1:** Unity Projesi Oluştur ve URP 2D yapılandır → Doğrula: `Project Settings` içinde 2D Renderer aktif mi?
- [ ] **Görev 2:** Sahne hiyerarşisini kur (Splash -> Menu -> LevelSelection) → Doğrula: Sahneler arası basit geçiş çalışıyor mu?

### Faz 2: Çekirdek Mekanikler (Core Gameplay)
- [ ] **Görev 3:** Dokunma tabanlı etkileşim sistemi (Tap to Play) → Doğrula: Objeye tıklandığında konsolda log ve görsel tepki veriyor mu?
- [ ] **Görev 4:** Ses Yönetim Sistemi (Voiceover Queue) → Doğrula: Birden fazla ses üst üste binmeden sırayla çalıyor mu?

### Faz 3: İçerik Geliştirme (Content)
- [ ] **Görev 5:** "Çiftlik Hayvanları" seviyesini oluştur → Doğrula: 5 farklı hayvan animasyonu ve sesi hazır mı?
- [ ] **Görev 6:** Basit ödül sistemi (Konfeti/Yıldız animasyonu) → Doğrula: Bir görev bittiğinde görsel kutlama tetikleniyor mu?

### Faz X: Doğrulama (Verification)
- [ ] **Görev 7:** Mobil Performans Testi (Unity Profiler) → Doğrula: < 16ms kare süresi.
- [ ] **Görev 8:** UX Audit (3-5 yaş uygunluğu) → Doğrula: checklist.py ile doğrulama.

---

## ✅ PHASE X COMPLETE
- Build: ⬜
- Security: ⬜
- Performance: ⬜
- Date: [Henüz Başlanmadı]
