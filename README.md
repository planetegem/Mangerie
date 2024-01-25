Mangerie is a Keleidoscope simulator written in C#, using the WPF framework. This is the first project I'm doing in C#/WPF, so I had to feel my way around quite a bit.

Structure:
1) There is a User control called Facet, which represents 1 reflection in the Kaleidoscope; In principle, this is just an image with a clip path, rotation and scale applied to it.
2) Depending on the mirror angle selected, the clip path in the Facet is changed (smaller angle = smaller section of the image visible)
3) The facet is then mirrorred once & patched onto a copy of itself
4) We copy this group around the center of the image

There is also a bonus game here: MazeMachine. There is only 1 reason for this game: when learning the syntax of a language, I always try some maze generation algorithms to familiarize myself with its data structure.
