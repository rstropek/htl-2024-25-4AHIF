# AI-Assistant for Guiding Students Through the “Diplomarbeitsdatenbank (ABA)” Form

## Background

At HTL-Leonding every diploma thesis (*Diplomarbeit*) must be registered in Austria’s *Diplomarbeitsdatenbank* (ABA). Although the school wiki (excerpt above) clarifies each ABA field, students still confuse wording, character limits, and style requirements. The purpose of this exercise is for computer-science students to design an AI support system that interacts with users (other students) and helps them create high-quality, rule-compliant ABA entries while preserving the students’ authorship.

## Tasks to Fulfil

### User-Experience and Interface Design

Map the complete interaction:

1. Sign-in/authentication (if needed).
2. Entry of raw notes (free text or per-field, you choose).
3. Generation of initial AI drafts.
4. Iterative review.
5. Validation highlighting rule violations (if needed).
6. Final export or copy-paste into ABA.

Produce annotated wireframes or a clickable mock-up using an AI tool (e.g. Lovable).

### System-Prompt and Prompt-Engineering Strategy

Create a system prompt that:

* Sets the assistant’s role.
* Embeds the constraints.

Describe how few-shot examples, dynamic placeholders (e.g. student names, thesis domain), and tool-like functions (e.g. a length checker) are incorporated.

Create and test your prompts in a console app based on:

* https://github.com/rstropek/openai-responses/blob/main/011-basics-dotnet/Program.cs (simple, works in .NET prior to .NET 10)
* https://github.com/rstropek/openai-responses/blob/main/031-streaming-dotnet/Program.cs (advanced, uses upcoming .NET 10 features)

### AI Model Selection and Hosting Option

Select an AI model that you software will use. Describe the selection process. Justify your choice.

### Back-End and Database Design

If your solution requires a database, provide an entity-relationship or class diagram.

Choose between a relational (e.g. PostgreSQL) or document (e.g. MongoDB) store and justify.

### Authentication and Authorisation

If your solution requires user accounts, propose an authentication flow that fits the school’s existing infrastructure.

### Rough Cost Estimation (100 students per year)

* State all assumptions (sessions per student, average tokens per request/response, number of drafts, storage requirements).
* Estimate and itemise costs.
* Present total annual expenses and highlight main cost drivers.

### Non-Functional Requirements

* List at least five measurable NFRs.
