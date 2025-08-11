(function(){
    document.addEventListener('DOMContentLoaded', function () {
        var openBtn = document.getElementById('open-video-modal');
        var modal = document.getElementById('video-modal');
        var closeBtn = document.getElementById('close-video-modal');
        var iframe = document.getElementById('video-iframe');
        var youtubeUrl = "https://www.youtube.com/embed/LL35TBsClH4?autoplay=1";

        openBtn.addEventListener('click', function () {
            modal.style.display = 'flex';
            iframe.src = youtubeUrl;
        });

        closeBtn.addEventListener('click', function () {
            modal.style.display = 'none';
            iframe.src = "";
        });

        // Optional: close modal when clicking outside the content
        modal.addEventListener('click', function (e) {
            if (e.target === modal) {
                modal.style.display = 'none';
                iframe.src = "";
            }
        });
    });
})();