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

[TEST] Manuellt
ID: TM-BID-03
Testfall: Auktion utan bud ger tom lista
Teknik: Gränsvärdesanalys (0 bud)
Förutsättningar: Auktion 2 finns, inga bud
Steg: 1. GET /api/auctions/2/bids
Testdata: id = 2
Förväntat: 200 OK, []
Faktiskt: 200 OK, []
Resultat: PASS
Branch/PR: feature/budregler (lokal)
Vem: Danial, 2026-11-27

[TEST] Manuellt
ID: TM-BID-04
Testfall: Ogiltigt auktions-id (bokstäver)
Teknik: Ekvivalensklass (ogiltig indata, ej heltal)
Förutsättningar: API:et körs
Steg: 1. GET /api/auctions/abc/bids
Testdata: id = abc
Förväntat: 404, ingen krasch
Faktiskt: 404 Not Found
Resultat: PASS
Branch/PR: feature/budregler (lokal)
Vem: Danial, 2026-11-27
