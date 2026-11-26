[TEST] Manuellt
ID: TM-BID-01
Testfall: Visa bud för auktion som inte finns
Teknik: Ekvivalensklass (ogiltigt auktions-id)
Förutsättningar: Tom databas, auktion 1 finns inte
Steg: 1. GET /api/auctions/1/bids
Testdata: id = 1
Förväntat: 404
Faktiskt: 404
Resultat: PASS
Branch/PR: feature/budregler (lokal, samma som dev)
Vem: Danial, 2026-11-26

[TEST] Manuellt
ID: TM-BID-02
Testfall: Bud visas med högsta budet först
Teknik: Ekvivalensklass (giltig auktion med flera bud)
Förutsättningar: Auktion 1 finns med bud 100, 300, 200 (inlagda i den ordningen)
Steg: 1. GET /api/auctions/1/bids
Testdata: id = 1
Förväntat: 200 OK, ordning 300, 200, 100
Faktiskt: 200 OK, ordning 300, 200, 100
Resultat: PASS
Branch/PR: feature/budregler (lokal, samma som dev)
Vem: Danial, 2026-11-26
