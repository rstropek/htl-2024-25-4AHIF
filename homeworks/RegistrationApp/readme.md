# Schnuppertag-API

## Hintergrundinformationen

Unsere Schule bietet regelmäßig "Schnuppertage" und "Schnupper-Workshops" an. Diese Veranstaltungen richten sich an interessierte Schülerinnen und Schüler, die einen Einblick in den Schulalltag erhalten möchten. Die Teilnehmerinnen und Teilnehmer können entweder einen Tag lang am regulären Unterricht teilnehmen oder spezifische Workshops besuchen, die die Fachrichtungen der Schule vorstellen.

Ihre Aufgabe besteht in der Konzeption und Implementierung einer Web-API, die als Backend für ein Web-Frontend dient. Die Anwendung soll folgende Funktionen abdecken:

1. Administrationsbereich
   * Darf nur von autorisierten Lehrerinnen und Lehrern unserer Schule genutzt werden (die Authentifizierung erfolgt über Microsoft Entra ID)
   * Schnuppertag-Kampagnen anlegen, bearbeiten, aktivieren, deaktivieren und löschen
   * Termine zu Schnuppertag-Kampagnen zuordnen, bearbeiten, aktivieren, deaktivieren und löschen
   * Anmeldungen online ansehen, herunterladen, Status ändern und löschen (im Fall von Abmeldungen)
2. Benutzerbereich (ohne Authentifizierung)
   * Schnuppertag-Kampagnen, die gerade aktiv sind, ansehen
   * Termine zu Schnuppertag-Kampagnen ansehen
   * Anmeldung zu einem Schnuppertag-Termin

## Logisches Datenmodell

### Kampagnen

Schnuppertage werden zu Kampagnen zusammengefasst. Beispiele für Kampagnen können sein:

* "Dein Schultag an der HTL Leonding (WS 2023/24)"
* "Elektronik-Schnupperworkshop Herbst 2024"
* "DDP-Schnupperworkshop Frühling 2025"

Je Kampagne sind folgende Informationen hinterlegt:

* Name
* Name des Ansprechpartners oder der Ansprechpartnerin (Lehrer oder Lehrerin; Verweis auf Benutzer in Microsoft Entra ID)
* Optional: Mindestanteil der für Mädchen reservierten Plätze in %
* Status (aktiv, inaktiv)
* Löschdatum

Es gibt folgende Logik-Regeln zu Kampagnen:

* Eine Kampagne kann nur dann aktiviert werden, wenn sie mindestens einen Termin hat.
* Eine Kampagne kann nur gelöscht werden, wenn es keine Anmeldungen gibt.
    * Wird eine Kampagne gelöscht, werden alle zugehörigen Termine ebenfalls gelöscht.
* Wird das Löschdatum erreicht, wird die Kampagne inklusive aller zugehörigen Termine und Anmeldungen gelöscht. Diese Operation wird protokolliert. Im Protokoll wird der Name der Kampagne und das Löschdatum festgehalten. Außerdem wird eine Benachrichtigung an die E-Mail-Adresse des Ansprechpartners oder der Ansprechpartnerin versendet.
* Das Löschdatum kann nie in der Vergangenheit liegen.
* Das Löschdatum darf nicht vor einem zugeordneten Termin gelegt werden.

### Termine

Zu jeder Kampagne gibt es 1..n Termine. Ein Termin besteht aus:

* Datum
* Optionale Zeitspanne (Start- und Endzeit)
* Status
    * Aktiv (Anmeldungen möglich)
    * Inaktiv (auch für normale Anwenderinnen und Anwender sichtbar, jedoch keine Anmeldungen möglich)
    * Verborgen (für normale Anwenderinnen und Anwender nicht sichtbar)

Zu jedem Termin gibt es 1..n Abteilungszuordnungen. Bei der Kampagne "Dein Schultag an der HTL Leonding (WS 2023/24)" könnte es sein, dass sich interessierte Jugendliche pro Termin zwischen der Informatik- und der Medientechnikabteilung entscheiden können. Bei der Kampagne "DDP-Schnupperworkshop Frühling 2025" ist nur eine Abteilung hinterlegt. Die Abteilungszuordnung kann je Termin einer Kampagne unterschiedlich sein (z.B. könnte es bei einem Termin Informatik und Medientechnik geben, bei einem anderen nur Informatik).  In der Mehrzahl der Fälle gibt es pro Termin nur eine Abteilungszuordnung. Je Abteilungszuordnung sind folgende Informationen hinterlegt:

* Abteilung (Freitext)
* Gesamtanzahl der freien Plätze zu diesem Termin in dieser Abteilung
* (Optional) Mindestanteil an für Mädchen reservierte Plätze in %

Es gibt folgende Logik-Regeln zu Terminen:

* Ein Termin kann nur gelöscht werden, wenn es keine Anmeldungen gibt.
* Wird ein Termin gelöscht, werden alle zugehörigen Abteilungszuordnungen ebenfalls gelöscht.
* Das Datum darf bei der Anlage eines Termins nicht in der Vergangenheit liegen.
* Termine können auch aktiviert werden, wenn die zugehörige Kampagne inaktiv ist. Die Termine werden zur Anmeldung jedoch erst dann freigegeben, wenn auch die Kampagne aktiv ist.
* Alle Termine einer Kampagne müssen vor dem Löschdatum der Kampagne liegen.

### Anmeldungen

Zu jedem Termin gibt es 0..n Anmeldungen. Eine Anmeldung besteht aus:

* Verweise
    * Verweis auf Kampagne
    * Verweis auf Termin
    * Verweis auf Abteilungszuordnung
* Zeitstempel der Anmeldung
* Vorname der Schülerin oder des Schülers
* Nachname der Schülerin oder des Schülers
* E-Mail-Adresse der Schülerin oder des Schülers
* Optional: Zweite E-Mail-Adresse (z.B. Eltern)
* Optional: Telefonnummer der Schülerin oder des Schülers
* Optional: Telefonnummer der Eltern
* Name der aktuell besuchten Schule
* Aktuelle Schulstufe (5-8, sonstige)
* Geschlecht der Schülerin oder des Schülers. Auswahlmöglichkeiten:
    * Männlich
    * Weiblich
    * Keine Angabe
* Status der Anmeldung
    * Platz fixiert
    * Warteliste

Jede Anmeldung muss eine kurze, leicht zu notierende ID zugewiesen bekommen. Diese muss aus 6 Großbuchstaben und am Ende zwei Ziffern bestehen, wobei leicht zu verwechselnde Buchstaben bzw. Ziffern (O und 0, I und 1, Z und 2, S und 5, B und 8) nicht verwendet werden dürfen. Um die ID leicht vorlesen zu können, müssen Konsonanten und Vokale abwechselnd vorkommen (z.B. "KATANA12", "FIREKI34"). Die ID wird später im User Interface inkl. QR-Code angezeigt (beispielsweise bei Anmeldung über einen Kiosk-PC) und kann von Schülerinnen und Schülern zur Verwaltung ihrer Anmeldung (z.B. nachsehen der Anmeldedetails,  Anmeldung stornieren) verwendet werden.

Es gibt folgende Logik-Regeln zu Terminen:

* Wird eine Anmeldung von einem Administrator oder einer Administratorin gelöscht, wird diese Operation protokolliert. Dabei wird die ID der Anmeldung, der Zeitstempel der Löschung und der ausführende Benutzer bzw. die ausführende Benutzerin protokolliert (Verweis auf Benutzer in Microsoft Entra ID). Außerdem wird eine Benachrichtigung an die angegebenen E-Mail-Adressen versendet.
* Bei Löschung der Anmeldung werden alle personenbezogenen Daten gelöscht. Nur die ID der Anmeldung bleibt im Protokoll.
* Eine Anmeldung wird bei der Anlage auf den Status "Warteliste" gesetzt, wenn es keine freien Plätze mehr gibt.
    * Für Mädchen werden dabei die gesamten freien Plätze berücksichtigt.
    * Für andere Geschlechter werden die freien Plätze abzüglich der für Mädchen reservierten Plätze berücksichtigt.
* Setzt ein Administrator oder eine Administratorin eine Anmeldung von "Warteliste" auf "Platz fixiert", wird diese Operation protokolliert. Dabei wird die ID der Anmeldung, der Zeitstempel der Änderung und der ausführende Benutzer bzw. die ausführende Benutzerin protokolliert (Verweis auf Benutzer in Microsoft Entra ID). Außerdem wird eine Benachrichtigung an die angegebenen E-Mail-Adressen versendet.

## Geforderte Operationen

* Die notwendigen Operationen für die oben genannten Tätigkeiten sind von den oben genannten Beschreibungen abzuleiten. Die folgende Liste an Operationen sind darüber hinaus gefordert.
* Anmeldeliste
    * Verpflichtender Kampagnenfilter
    * Optionaler Termin- und Abteilungsfilter
    * Getrennte Darstellung von fixierten Plätzen und Wartelistenplätzen. Fixierte Plätze sind nach Namen sortiert, Wartelistenplätze nach Zeitstempel der Anmeldung.
* Statistikliste
    * Optionaler Kampagnenfilter
    * Je Kampagne, Termin und Abteilung:
        * Anzahl fixierter Plätze
        * %satz der fixierten Plätze für Mädchen
        * Anzahl Wartelistenplätze
* Detailabfrage
    * Verpflichtender Anmeldungsfilter (über ID)
    * Rückgabe aller Daten über die Anmeldung
* Protokollabfrage
    * Verpflichtender Zeitraumfilter
    * Rückgabe aller Protokolleinträge
* Anstoßen der Lösung von Kampagnen, deren Löschdatum erreicht wurde
    * Diese Operation wird von einem regelmäßigen Job angestoßen
* Für nicht authentifizierte Benutzer:
    * Anzeige der aktiven Kampagnen
    * Anzeige der Termine zu einer Kampagne (inkl. Abteilungszuordnungen)
        * Dieser Schritt ist erst möglich, wenn die persönlichen Daten der Schülerin oder des Schülers eingegeben wurden
        * Optional: Nur Anzeige von Terminen, die noch freie Plätze haben

## Technische Rahmenbedingungen

* RESTful Web API
* Authentifizierung über Microsoft Entra ID
    * Ist in dieser Übung nur konzeptionell vorzusehen, muss aber nicht umgesetzt werden
* Speicherung der Daten in JSON-Dateien
    * Es ist im ersten Schritt nicht notwendig, die Daten in einer Datenbank zu speichern
    * Je Kampagne ist eine JSON-Datei anzulegen, die alle Informationen zu dieser Kampagne enthält
    * Die Protokolldatei ist eine CSV-Datei
* Implementierung mit C# und ASP.NET Core Minimal API

## Bonusaufgaben

* Dokumentation der RESTful Web API mit Swagger (Open API Specification)
* Gleichzeitiger Schreibzugriff auf eine Datei muss über [locking](https://learn.microsoft.com/en-us/dotnet/api/system.io.filestream.lock) verhindert werden
* Unit Tests
* Integrationstests
* ReCAPTCHA-Integration bei der Anmeldung
* Implementierung der Authentifizierung über Microsoft Entra ID
