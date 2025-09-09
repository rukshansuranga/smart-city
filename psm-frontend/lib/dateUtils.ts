/**
 * Utility functions for date formatting
 */

/**
 * Formats a JavaScript Date object for NodaTime LocalDateTime backend
 * NodaTime LocalDateTime expects format: YYYY-MM-DDTHH:MM:SS (without timezone)
 * @param date - The JavaScript Date object to format
 * @returns Formatted date string compatible with NodaTime LocalDateTime
 */
export const formatNodaDateTime = (date: Date): string => {
  // NodaTime LocalDateTime expects format: YYYY-MM-DDTHH:MM:SS (without timezone)
  // This creates a local datetime without timezone information
  const year = date.getFullYear();
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const day = date.getDate().toString().padStart(2, "0");
  const hours = date.getHours().toString().padStart(2, "0");
  const minutes = date.getMinutes().toString().padStart(2, "0");
  const seconds = date.getSeconds().toString().padStart(2, "0");

  return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;

  // Alternative: If backend accepts ISO without 'Z', use this instead:
  // return date.toISOString().replace('Z', '');
};

/**
 * Formats a JavaScript Date object for NodaTime LocalDate backend (date only)
 * @param date - The JavaScript Date object to format
 * @returns Formatted date string in YYYY-MM-DD format
 */
export const formatNodaDate = (date: Date): string => {
  const year = date.getFullYear();
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const day = date.getDate().toString().padStart(2, "0");

  return `${year}-${month}-${day}`;
};
