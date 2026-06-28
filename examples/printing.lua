-- EXAMPLE: print text

API.out.info("Normal message")
API.out.error("Cause of error here", "Error message") -- Colored red
API.out.warning("Cause of warning", "Warning") -- Colored yellow

print("Raw messages") -- No decorations or coloring.