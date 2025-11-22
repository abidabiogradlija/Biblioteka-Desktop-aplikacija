# Biblioteka – Desktop Aplikacija (.NET 8 + SQL Server)

Ovaj projekat predstavlja jednostavnu desktop aplikaciju za upravljanje bibliotekom, izrađenu korištenjem C# WinForms i SQL Server baze podataka.

## 📌 Funkcionalnosti
- Upravljanje autorima (CRUD)
- Upravljanje knjigama (CRUD)
- Upravljanje članovima (CRUD)
- Upravljanje posudbama
- Evidencija rokova i statusa vraćanja knjiga

---

# 🛠 Uputstvo za pokretanje aplikacije na drugom računaru

Da bi aplikacija radila ispravno, potrebno je podesiti SQL Server bazu na način opisan u nastavku.

## 1️⃣ Instalirati SQL Server i SSMS  
Korisnik mora imati instalirane:
- **SQL Server Management Studio (SSMS)**

## 2️⃣ Kreirati bazu *BibliotekaDB*
U SSMS-u pokrenuti:

```sql
CREATE DATABASE BibliotekaDB;
```

## 3️⃣ Importovati dostavljenu SQL skriptu  
U folderu projekta nalazi se datoteka **BibliotekaDB.sql**.  
U SSMS-u uraditi:

1. Otvoriti *BibliotekaDB*
2. Kliknuti **New Query**
3. Zalijepiti sadržaj skripte
4. Kliknuti **Execute**


## 4️⃣ Ažurirati connection string  
U `Form1.cs` promijeniti ime računara u svom connection stringu:

```csharp
"Data Source=IME-RACUNARA;Initial Catalog=BibliotekaDB;Integrated Security=True";
```


## 5️⃣ Pokrenuti aplikaciju  
Nakon pravilnog podešavanja baze:
- otvara se WinForms GUI,
- učitavaju se svi podaci,
- sve CRUD operacije će raditi ispravno.

---

# 📂 Sadržaj repozitorija

- `README.md` — uputstvo za pokretanje
- `.sql fajl` — kompletna skripta za kreiranje baze
- Izvorni kod aplikacije (`*.cs` datoteke)

---

# ✔ Napomena
Za potpuno funkcionalno izvođenje aplikacije, korisnik mora imati aktivan SQL Server instance i ispravno podešen connection string.
