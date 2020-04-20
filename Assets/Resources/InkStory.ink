VAR location = "house" 
VAR reaction = "scared"
VAR num = 3433

* [apartment]
~ location = "apartment"
-> choose_reaction

* [house]
-> choose_reaction

===choose_reaction===
* [calm]
~ reaction= "calm"
->main0

* [scared]
->main0

===main0===
Trapped and hopeless, one of us was going to lose.

NEWLINE

I knew I was making a mistake as soon as the old wooden floor of his {location == "apartment": apartment | house} creaked beneath my shaking feet. 

NEWLINE

I desperately needed the money and promised my sweet sister that I’d get us out of our hell hole of a home.

NEWPAGE

I wasn’t your typical delinquent teen despite what others may believe; I just wanted a fresh start. 

NEWLINE

Yet another night had passed and I was still shaken up after my father’s return from the bar, the regular bruises, broken glass, tears and spite remained. 

NEWPAGE

{location == "apartment": I had escaped and was seated in the café underneath our old grey building, the regular waiter handing me a cup of tea with a sad smile. | I had escaped and was seated in the old diner down the street.} 

NEWLINE

As the warm tea flowed down my throat and the tears seeped into my t-shirt, I noticed a newspaper on the seat next to me. 

NEWPAGE

I mindlessly flipped through its faded pages and read “Medically retired veteran receives a $500,000 cash settlement over the tragic death of his only daughter”. 

NEWLINE

The paper went on to discuss how the drunken driver was only serving a miniscule amount of prison time as she came from a wealthy… the words began to blur… imagine all the things I could do with this money… I could finally escape.

NEWPAGE

I spent the next day doing my research and finally found the old man’s address. 

NEWLINE

{location == "apartment": The funeral had been held in the cemetery right behind his apartment building. | The funeral had been held in the garden of his abandoned-looking home.} 

NEWLINE

I plotted and concluded that this would be easily done as he was an elderly man with some kind of medical condition. I was there the next day.

NEWPAGE


{location == "apartment": -> apartment1 | -> house1}

===apartment1===

Passing by the zooming cars, I speedwalked to the cemetery’s address… How will I find which apartment he lived in? 

NEWLINE

A sigh of relief came over me as I noticed a bunch of sympathy flowers on the 5th floor’s fire escape. He must live there.

NEWLINE

As I was walking up the stairs on the fire escape, I felt my arm make contact with some sticky texture. 

NEWPAGE 

I immediately turned my head to see what it was and found that I walked into a shiny spider web.  

NEWLINE

{reaction == "scared": -> scared1 | -> calm1}

===house1===

The atmosphere was damp and peculiar noises filled the air around his home, eerily separating the property from its surroundings.

NEWLINE

As I was creeping around the bushes surrounding his home, I felt my arm make contact with some sticky texture.

NEWLINE

I immediately turned my head to see what it was and found that I walked into a shiny spider web.

NEWPAGE

{reaction == "scared": -> scared1 | -> calm1}

===scared1===
Panic rose in my body and I felt goose bumps forming on my skin, I could not bear the thought of encountering a hairy eight legged vermin right now.

NEWPAGE

I tried to balance my breathing, ignored the voices in my head that told me to turn around and go back and focused on my goal: drug him, get the money, leave and restart.

NEWPAGE

->main

===calm1===
 I breathed out a sigh of relief and laughed at myself for getting so riled up for nothing.

NEWLINE

Seeing a spider would be the least of my issues now. 

NEWPAGE

I shook my head and focused on my goal: drug him, get the money, leave and restart.
 
NEWPAGE
 
 -> main


===main===

The same small build that I’ve despised my whole life proved itself handy as I entered his home through the kitchen window. 

NEWLINE

I took a few moments to get accustomed to the darkness and looked around trying to determine where his room may be.

NEWPAGE

<color=b5651d>Tossing and turning, he was restless.</color>

NEWLINE

<color=b5651d>His love, his daughter, the only one that has taken care of him in his hardest times was gone.</color>

NEWLINE

<color=b5651d>He wondered: 'What’s the meaning of it all?'</color>

NEWLINE

<color=b5651d>He had always been strong for her, he didn’t want her to feel sorry for him, she was supposed to live her life fully and enjoy it to the very end.</color>

NEWPAGE

<color=b5651d>How could someone so vibrant and full of light be lying lifeless in a grave? </color>

NEWPAGE

I closed my eyes and listened for signs of life, after a few moments I heard light breathing from down the hall. 

NEWLINE

I removed my shoes, secured my backpack and put the drugged chloroform cloth in a small plastic bag in my pocket.

NEWPAGE

I tiptoed towards the sound of his breathing and found a white haired elderly man lying straight on his back. 

NEWLINE

His hardened skin demonstrated his struggles, but despite having a strong form and a firm demeanor, his eyes, even in sleep, bared his sadness.

NEWPAGE

His sweet daughter had been replaced by a pile of money. 

NEWLINE

Did they think that he could buy happiness? 

NEWLINE

Maybe he could use it to build the orphanage that she had always been talking about. 

NEWLINE

It would honor her name. He just wished he was able to say “goodbye”. 

NEWPAGE

I cautiously entered his room and noticed a large portrait of him and a young woman smiling genuinely, she must’ve been his daughter and it looked like he loved her dearly. 

NEWLINE

I then noticed a few holes on his walls; it looked like they’d been punched through. 

NEWLINE

I quickly turned to check whether his hands were bloody and found him staring at me with eyes wide open. 

NEWPAGE

All the blood drained from my body at that moment and my mouth went dry. 

NEWPAGE

Thump, thump, thump.

NEWPAGE

My heart was beating hastily and my body was shaking with dread. 

NEWLINE

Was I going to die?

NEWLINE

He calls me by a name that is not my own.

NEWLINE

Should I run?

NEWLINE

Should I walk towards him and pretend to be her? 

NEWPAGE

Should I try drugging him now?

NEWLINE

Am I going to get out of this alive?

NEWLINE

As I was on the verge of fainting, he suddenly falls back to his former position with tears on his cheeks and I realize that he was merely dreaming. I’m alright.

NEWPAGE

Now do I go back to my initial plan or do I bolt?


-> END