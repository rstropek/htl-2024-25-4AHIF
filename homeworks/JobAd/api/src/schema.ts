import { z } from 'zod';

export const jobAdSchema = z.object({
  title: z.string(),
  textEN: z.string(),
});

export const jobAdWithIdSchema = jobAdSchema.extend({
  id: z.number(),
});

export type JobAd = z.infer<typeof jobAdWithIdSchema>;

export const jobAdTranslationSchema = z.object({
  jobAdId: z.number(),
  language: z.string(),
  translatedText: z.string(),
});

export type JobAdTranslation = z.infer<typeof jobAdTranslationSchema>;

export const jobAdWithTranslationsSchema = jobAdWithIdSchema.extend({
  translations: z.array(z.object({ language: z.string(), translatedText: z.string() })),
});

export type JobAdWithTranslations = z.infer<typeof jobAdWithTranslationsSchema>;

export const jobAdTranslationWithIdSchema = jobAdTranslationSchema.extend({
  id: z.number(),
});

export const translateRequestSchema = z.object({
  text: z.string(),
  source_lang: z.string(),
  target_lang: z.string(),
});

export const translatedTextResponseSchema = z.object({
  translation: z.string(),
});
