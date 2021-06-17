const glide2 = document.getElementById("glide2");
const slider5 = document.getElementById("glide_5");
if (glide2)
    new Glide(glide2, {
        type: "carousel",
        startAt: 0,
        perView: 5,
        hoverpause: false,
        autoplay: 2000,
        animationDuration: 800,
        animationTimingFunc: "cubic-bezier(0.165, 0.840, 0.440, 1.000)",
        breakpoints: {
            1200: {
                perView: 3,
            },
            768: {
                perView: 2,
            },
        },
    }).mount();

if (slider5) {
    new Glide("#glide_5", {
        type: "carousel",
        startAt: 0,
        perView: 3,
        rewin: false,
        autoplay: 3000,
        animationDuration: 800,
        animationTimingFunc: "cubic-bezier(0.165, 0.840, 0.440, 1.000)",
        breakpoints: {
            998: {
                perView: 2,
            },
            768: {
                perView: 1,
            },
        },
    }).mount();
}


