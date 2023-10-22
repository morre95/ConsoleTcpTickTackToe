# Tic-Tac-Toe med TCP Server och Klient

Detta är en implementation av spelet Tic-Tac-Toe (Tre i Rad) som en skoluppgift i C# med användning av TCP för kommunikation mellan en server och en klient. Spelet har en 3x3 spelplan och erbjuder tre svårighetsgrader: Easy, Normal och Hard. Dessutom har projektet en komprimeringsmekanism för att optimera bandbreddsanvändningen för kommunikationen mellan servern och klienten.

## Så här kör du programmet

### Steg
1. Klona repot:
   ```shell
   git clone https://github.com/morre95/ConsoleTcpTickTackToe.git
2. Build solution genom Visual Studio

För att köra spelet behöver du starta både servern och klienten. 

Servern kommer nu att lyssna på en specifik port och vänta på anslutningar från klienten.
Klienten kommer att försöka ansluta till servern. Det första som kommer upp är val mellan de olika svårighetsgraderna.

## Svårighetsgrader

Detta spel har tre svårighetsgrader:

- Easy: Servern tar enkla slumpmässiga drag.
- Normal: Servern gör mer strategiska drag, men är inte övermäktig.
- Hard: Servern är svår att besegra och tar nästan alltid det bästa möjliga draget.

## Komprimerad kommunikation

För att optimera bandbreddsanvändningen använder detta projekt GZip komprimering för att minska datamängden som skickas mellan servern och klienten. Detta säkerställer att spelet fungerar smidigt även med långsamma nätverksanslutningar. Vilket man sällan har i ett lokalt nätverk. Men det var uppgiften så därför finns det med.

## Bidragande

Om du vill bidra till projektet eller rapportera buggar, är du välkommen att öppna en GitHub-issue eller skicka in en pull-begäran.

## Licens

Det här projektet är licensierat under MIT-licensen. Se [LICENSE](LICENSE) för detaljer.
