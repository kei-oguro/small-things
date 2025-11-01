[Parent](../README.md)

# Try using gpt-oss with llama.cpp

I try using gpt-oss with llama.cpp on WSL on Windows11.

At first, I tried it on Fedora on WSL, but I encountered an error that might have been driver-related. In this step, I ran the downloaded llama.cpp, then built it myself to let it use the CUDA feature.

Next, I did the same thing on Ubuntu in WSL. It went smoothly because I had already done it the first try.

Then, I tried different parameter combinations for gpt-oss-20B runs on llama.cpp.

Finally, I can do it again by following [my second try on Ubuntu](#re-try-on-wsl-ubuntu-2404) and running the command line `llama-cli -hf ggml-org/gpt-oss-20b-GGUF --n-cpu-moe`.

## First Try

1.  [cmd] `wsl --install FedoraLinux-43 --name Try-gpt-oss`
2.  [Fedora43] `sudo dnf install etckeeper`
3.  [Fedora43] Edit a configuration file `sudo nano /etc/etckeeper/etckeeper.conf`
    - Comment in `AVOID_DAILY_AUTOCOMMITS=1`
4.  [Fedora43] `sudo etckeeper init`
5.  [Fedora43] `sudo etckeeper commit "initial commit"`
6.  [Fedora43] `sudo dnf install toolbox`
7.  [Fedora43] `toolbox create 1st-try`
8.  [Fedora43] `toolbox enter 1st-try`
9.  [toolbx 1st-try] `sudo dnf install etckeeper`
10. [toolbx 1st-try] `sudo etckeeper init`
11. [toolbx 1st-try] `sudo etckeeper commit "initial commit"`
12. [toolbx 1st-try] `curl -L https://nixos.org/nix/install | sh`
13. [toolbx 1st-try] `nix profile --extra-experimental-features nix-command --extra-experimental-features flakes install
nixpkgs#llama-cpp`
14. [toolbx 1st-try] `llama-cli -hf ggml-org/gpt-oss-20b-GGUF`

    - After download the model, it shown as follow.
      ```
      == Running in interactive mode. ==
       - Press Ctrl+C to interject at any time.
       - Press Return to return control to the AI.
       - To return control without starting a new line, end your input with '/'.
       - If you want to submit another line, end your input with '\'.
       - Not using system message. To change it, set a different value via -sys PROMPT
      ```
    - Then a simple `>` prompt appeared, so I entered a blank line. gpt-oss then continued answering from where it had left off, explaining how to implement an Electron application.
    - Although the answer was not finished, it did stop with the following output.

      ```
      main: context full and context shift is disabled => stopping


      llama_perf_sampler_print:    sampling time =     489.36 ms /  4096 runs   (    0.12 ms per token,  8370.08 tokens per second)
      llama_perf_context_print:        load time =    2695.05 ms
      llama_perf_context_print: prompt eval time =       0.00 ms /     1 tokens (    0.00 ms per token,      inf tokens per second)
      llama_perf_context_print:        eval time = 1070899.50 ms /  4095 runs   (  261.51 ms per token,     3.82 tokens per second)
      llama_perf_context_print:       total time = 5643468.13 ms /  4096 tokens
      llama_perf_context_print:    graphs reused =       4079
      llama_memory_breakdown_print: | memory breakdown [MiB] | total   free     self   model   context   compute    unaccounted |
      llama_memory_breakdown_print: |   - Host               |                 12048 = 11536 +     114 +     398                |
      ```

    - Downloaded model is at `~/.cache/llama.cpp/`.

15. [toolbx 1st-try] I tried `llama-server -hf ggml-org/gpt-oss-20b-GGUF -c 0 -fa --jinja` according to https://huggingface.co/ggml-org/gpt-oss-20b-GGUF . But it produced an unknown argument for `-fa` and an error in jinja template with `--jinja`. (p.s. -fa takes one argument, either 0 or 1. That was my mistake here)
16. [toolbx 1st-try] `llama-server -hf ggml-org/gpt-oss-20b-GGUF -c 0`
17. [Windows11] I opened the webpage at `http://localhost:8080` and asked gpt-oss how I could make the most of my RTX 2060 with 6 GB VRAM. It suggested as follow.
    1. Build the CUDA version llama.cpp.
    2. Quantize the model.
    3. Adjust the arguments for `--n-gpu-layers` and `--threads`. The optimal number of layers would be somewhere between 18 and 22.
18. Before follow it, I tried to run with `--n-gpu-layers` option, then it returned following message.
    ```
    warning: no usable GPU found, --gpu-layers option will be ignored
    warning: one possible reason is that llama.cpp was compiled without GPU support
    ```
    Okay, I've realized that I need to build it.
    - And, in this time, gpt-oss generated about 4 tokens per second. It was not fast, I could accept such a speed if answers were short, but gpt-oss is very verbose.
19. [toolbx 1st-try] `exit`
20. [Fedora] `toolbox create 2nd-try`
21. [Fedora] `toolbox enter 2nd-try`
22. [toolbx 2nd-try] `ls ~/.cache/llama.cpp` to confirm downloaded model can be accessed.
23. [toolbx 2nd-try] `llama-cli` to confirm that llama.cpp installed by nix doesn't exist.
24. [toolbx 2nd-try] `mkdir -p github/ggml-org && cd github/ggml-org`
25. [toolbx 2nd-try] `git clone https://github.com/ggml-org/llama.cpp.git && cd llama.cpp`
26. [toolbx 2nd-try] `sudo dnf install etckeeper`
27. [toolbx 2nd-try] `sudo etckeeper init`
28. [toolbx 2nd-try] `sudo etckeeper commit "initial commit"`
29. [toolbx 2nd-try] `sudo dnf install cmake gcc g++ python3 python3-pip nvidia-cuda-toolkit`
30. [toolbx 2nd-try] Failed to run `sudo dnf install nvidia-cuda-toolkit`, so followed the instructions https://rpmfusion.org/Howto/CUDA .
    1. `sudo dnf config-manager addrepo --from-repofile=https://developer.download.nvidia.com/compute/cuda/repos/fedora42/$(uname -m)/cuda-fedora42.repo`
    2. `sudo dnf clean all`
    3. `sudo dnf config-manager setopt cuda-fedora42-$(uname -m).exclude=nvidia-driver,nvidia-modprobe,nvidia-persistenced,nvidia-settings,nvidia-libXNVCtrl,nvidia-xconfig`
    4. `sudo dnf -y install cuda-toolkit xorg-x11-drv-nvidia-cuda`
       said that
       ```
       Failed to resolve the transaction:
       No match for argument: xorg-x11-drv-nvidia-cuda
       ```
       So I followed https://developer.nvidia.com/cuda-downloads?target_os=Linux&target_arch=x86_64&Distribution=Fedora&target_version=42&target_type=rpm_network
       1. Already done for `sudo dnf config-manager addrepo --from-repofile https://developer.download.nvidia.com/compute/cuda/repos/fedora42/x86_64/cuda-fedora42.repo`
       2. Already done for sudo `dnf clean all`
       3. `sudo dnf -y install cuda-toolkit-13-0`
       4. `sudo dnf -y install cuda-drivers`
31. [toolbx 2nd-try] Failed to run `cmake -B build -DGGML_CUDA=ON -DCMAKE_CUDA_ARCHITECTURES="75"` with `No CMAKE_CUDA_COMPILER could be found.`

    - "75" is the compute capability of RTX 2060 according to https://developer.nvidia.com/cuda-gpus .
    - Then add option and run `cmake -B build -DGGML_CUDA=ON -DCMAKE_CUDA_ARCHITECTURES="75" -DCMAKE_CUDA_COMPILER=/usr/local/cuda/bin/nvcc`

      shows that

      ```
      /usr/include/bits/mathcalls.h(206): error: exception specification is incompatible with that of previous function "rsqrt" (declared at line 629 of /usr/local/cuda/bin/../targets/x86_64-linux/include/crt/math_functions.h)
      ```

    - According to https://gist.github.com/joanbm/7960974c08bd7f7367a09e09e6b8935c , glibc must be below 2.42 . I modified with `sudo nano /usr/local/cuda-13.0/targets/x86_64-linux/include/crt/math_functions.h` to adding `noexcept (true)` to `rsqrt`.
    - Next, `Could NOT find CURL.  Hint: to disable this feature, set -DLLAMA_CURL=OFF`, so did `sudo dnf install libcurl-devel`
    - It was good to install `ccache`. `cmake` configuration suggested it, but I missed it until the building was completed.

32. [toolbx 2nd-try] `cmake --build build --config Release`
33. [toolbx 2nd-try] `build/bin/llama-cli --version` reports

    ```
    build/bin/llama-cli: error while loading shared libraries: libcuda.so.1: cannot open shared object file: No such file or directory
    ```

    1. `export PATH=/usr/local/cuda-13.0/bin${PATH:+:${PATH}}`
    2. `export LD_LIBRARY_PATH=/usr/local/cuda-13.0/lib64${LD_LIBRARY_PATH:+:${LD_LIBRARY_PATH}}`
    3. `rm -rf build`
    4. `sudo dnf install ccache`
    5. `cmake -B build -DGGML_CUDA=ON -DCMAKE_CUDA_ARCHITECTURES="75" -DCMAKE_BUILD_TYPE=Debug`
    6. `cmake --build build --config Debug`
    7. Stopped once, then added the thread count, then run `cmake --build build --config Debug -j8`
    8. The same libcuda.so.1 missing error occurred again.
    9. Gemini tells me that libcuda.so.1 must be `/usr/lib/swl/lib/`.
       1. [toolbx 2nd-try] `exit`
       2. [Fedora43] `ls /usr/lib/wsl/lib` => That was.
       3. [Fedora43] `toolbox enter 2nd-try`
    10. [toolbx 2nd-try] `export LD_LIBRARY_PATH=/run/host/usr/lib/wsl/lib && export PATH=/usr/local/cuda-13.0/bin${PATH:+:${PATH}}`
    11. [toolbx 2nd-try] `build/bin/llama-cli --version`
        ```
        ggml_cuda_init: failed to initialize CUDA: CUDA driver version is insufficient for CUDA runtime version
        register_backend: registered backend CUDA (0 devices)
        register_backend: registered backend CPU (1 devices)
        register_device: registered device CPU (Intel(R) Core(TM) i5-14500)
        load_backend: failed to find ggml_backend_init in /home/gptuser/github/ggml-org/llama.cpp/build/bin/libggml-cuda.so
        load_backend: failed to find ggml_backend_init in /home/gptuser/github/ggml-org/llama.cpp/build/bin/libggml-cpu.so
        version: 6884 (229bf6862)
        built with cc (GCC) 15.2.1 20251022 (Red Hat 15.2.1-3) for x86_64-redhat-linux
        ```
    12. [toolbx 2nd-try] `rm -rf build` and configure cmake for Debug and build it again. But `-j20`. And **it Failed**. I needed to make sure that the versions of the runtime and the toolkit matched.
    13. [Windows11] Installed the nVidia App and updated the GPU driver.
    14. | env       | smi        | Driver | CUDA |
        | --------- | ---------- | ------ | ---- |
        | Windows11 | 581.57     | 581.57 | 13.0 |
        | Fedora43  | 580.102.01 | 581.57 | 13.0 |
        | toolbox   | N/A        | N/A    | N/A  |

    15. Exited from WSL, shut is down, and re-entered.
    16. [Windows11] Did clean installation he nVidia GPU driver to follow an advice from Gemini.
    17. [Fedora43] `ls -la /usr/lib/wsl/lib/libcuda*` => `-r-xr-xr-x 4 root root   175248 Oct 10 14:36 /usr/lib/wsl/lib/libcuda.so.1` Gemini told me that libcuda.so.1 must be a symbolic link. Also said that size might be small so the wsl preparation for mounting drivers seems broken.
    18. `cmake -B build -DGGML_CUDA=ON -DCMAKE_CUDA_ARCHITECTURES="75" -j8`
    19. I gave up.

---

## Re-try on WSL Ubuntu-24.04

1. [cmd] `wsl --install Ubuntu gpt-ubuntu`
2. [cmd] `wsl gpt-ubuntu`
3. [Ubuntu24] `sudo apt update && sudo apt -y upgrade`
4. [Ubuntu24] `mkdir -p ~/github/ggml-org && cd ~/github/ggml-org`
5. [Ubuntu24] `sudo apt install cmake gcc g++ ccache python3 python3-pip nvidia-cuda-toolkit`
6. [Ubuntu24] `cmake -B build` reported `Could NOT find CURL (missing: CURL_LIBRARY CURL_INCLUDE_DIR)`
   - `sudo apt install libcurl4-openssl-dev`
7. [Ubuntu24] `cmake --build build --config Debug -j20`
8. [Ubuntu24] It was fine to run `build/bin/llama-cli -hf ggml-org/gpt-oss-20b-GGUF -p hello --n-gpu-layers 20` just on the CPUs. Off course I didn't specify the option to use GPUs, as the binary said `warning: no usable GPU found, --gpu-layers option will be ignored`.
9. [Ubuntu24] `cmake -B build -DGGML_CUDA=ON -DCMAKE_CUDA_ARCHITECTURES="75" `

Finally, gpt-oss ran on my GPU. It generated 6.26 tokens per second for a sample prompt. At this stage, it was too slow to use it for chatting.

I was surprised that the latest prompt and the answer were held on the page received from the llama-server. does this mean that the queries are stored in browser cookies? => It seems so because when the port number was changed, everything that had been saved disappeared.

It has been done smoothly. However, the appearance of the files in the /usr/lib/wsl/lib/cuda\* directory is very similar. Why is this acceptable on Ubuntu but not on Fedora?

---

I asked gpt-oss about tuning the llama.cpp options. This is both a tuning reference and a quality evaluation output sample.

<details><summary>This is the answer from the local gpt-oss-20B: "<strong>What adjustments should I make to run gpt-oss efficiently with llama.cpp on an RTX 2060 with 6 GB VRAM? What order should I try them in</strong>?"</summary>

> RTX 2060 6 GB VRAM で効率的に gpt-oss を llama.cpp で動かすには、何を調整する？どういう順番で試す？

---

We can give a step-by-step approach:

1. Build llama.cpp with GPU support: compile with CMake and set CUDA.
2. Choose a quantized model: 4-bit or 8-bit quantization.
3. Determine the model size: GPT-OSS or GPT-2 small.
4. Set n_gpu_layers: e.g., 24 layers loaded into GPU.
5. Set n_ctx: reduce context length if needed.
6. Use batch size: e.g., 1 or 2.
7. Use environment variable: "GGML_USE_CUBLAS=1".
8. Use "llama.cpp" command: "llama-cli -m model.bin -t 8 -ngl 24".

Also mention that 6GB VRAM is enough for 4-bit quantized GPT-2 but maybe not for GPT-4. Use "ggml" quantization.

We can mention that GPT-OSS can be loaded with "llama.cpp" if it's in the right format, but the user might need to convert it. Provide steps to convert from ONNX or other format to ggml.

We need to be mindful: The user might not know how to compile. Provide a quick guide: "git clone https://github.com/ggerganov/llama.cpp" then compile with "make -j$(nproc)". Then "make CUBLAS=1". Then run the model. Provide the recommended environment variables.

Ok, let's produce a detailed answer.<|end|><|start|>assistant<|channel|>final<|message|>## Quick‑Start: Getting GPT‑OSS to Run Efficiently on an RTX 2060 (6 GB)

Below is a practical, step‑by‑step checklist you can follow.
It assumes you’re comfortable with a terminal, `git`, and compiling a small C++ project.

> **TL;DR** – Use a _4‑bit_ quantized model, limit the number of GPU‑resident layers, and keep the context window small. Compile `llama.cpp` with CUDA support and run with `-ngl` (GPU layers) and `-n_ctx` (context length) tuned for 6 GB.

---

### 1. Prepare the Code Base

```bash
# Clone the repo
git clone https://github.com/ggerganov/llama.cpp
cd llama.cpp

# Build with CUDA (GPU) support
# You need a working CUDA toolkit (e.g. 12.x) installed.
make -j$(nproc) CUBLAS=1
```

- `CUBLAS=1` tells the build system to enable the cuBLAS backend.
- If you hit any compiler errors, make sure `nvcc` is on your `PATH` and that you have a recent CUDA toolkit.

---

### 2. Pick the Right Model & Quantization

| Model          | Size (MB) | Typical VRAM Needed (4‑bit) | Typical VRAM Needed (8‑bit) |
| -------------- | --------- | --------------------------- | --------------------------- |
| `gpt-oss-1.3B` | ~ 1 GB    | ~ 500 MB                    | ~ 1 GB                      |
| `gpt-oss-2.7B` | ~ 2 GB    | ~ 1 GB                      | ~ 2 GB                      |
| `gpt-oss-6B`   | ~ 4 GB    | ~ 2 GB                      | ~ 4 GB                      |

**Recommendation** – On a 2060, the 2.7B (or the 1.3B if you want to squeeze extra performance) model quantized to **4‑bit** is a sweet spot:

- You stay comfortably below 6 GB.
- The GPU can keep all layers in memory (you’ll see 24‑32 GB for a full 6 B model, which is impossible).

> **How to get the quantized file**
>
> 1. Download the plain FP16 model (e.g., `ggml-model-f16.bin`).
> 2. Run the quantizer:
>
> ```bash
> ./quantize ggml-model-f16.bin gpt-oss-2.7B-q4.bin 4
> ```
>
> 3. Use `gpt-oss-2.7B-q4.bin` in the next steps.

---

### 3. Decide on Context Length (`-n_ctx`)

- The default context window in many LLMs is 2048 tokens.
- Each token takes about 3 bytes in the GGML representation → ~6 KB per token.
- 2048 × 6 KB ≈ 12 MB – trivial compared to VRAM.
- However, if you run multiple inference threads or want to keep the GPU memory low, you can shrink it (e.g., 1024 or 512).

> **Rule of thumb** – For a single‑threaded inference, leave `-n_ctx` at 2048 unless you hit a memory warning.
> For multi‑threaded or batch inference, try 1024.

---

### 4. Choose How Many Layers to Load on GPU (`-ngl`)

`llama.cpp` will keep _all_ layers in RAM but will offload some to GPU memory if you ask.

- `-ngl 0` → no layers on GPU (CPU‑only).
- `-ngl 24` → first 24 layers on GPU (default for 4‑bit).

**Tip** – Set `-ngl` to the _maximum number of layers that fit in 6 GB_.
A quick estimate:

- 4‑bit per layer ≈ 1 MB (for a 2.7B model).
- 6 GB / 1 MB ≈ 600 layers → you can safely set `-ngl 100` or more.
- In practice, you’ll hit other limits (e.g., CPU RAM) before the GPU runs out.

---

### 5. Tune CPU Threads (`-t`) and Batch Size (`-n_batch`)

- `-t` controls the number of CPU threads used for the _CPU‑only_ part of the model.
  - On a quad‑core laptop, `-t 4` is a good start.
  - On a 16‑core desktop, you can bump to `-t 8` or `-t 12`.
- `-n_batch` controls the batch size for decoding tokens.
  - Small values (1–2) keep memory low.
  - Larger values (8–16) can speed up inference but use more VRAM for intermediate tensors.

> **Rule of thumb** – Start with `-t 8 -n_batch 2` on a 2060.

---

### 6. Run the Model

```bash
./main -m gpt-oss-2.7B-q4.bin -t 8 -ngl 24 -n_ctx 2048 -n_batch 2
```

If you want to test a prompt:

```bash
./main -m gpt-oss-2.7B-q4.bin -t 8 -ngl 24 -n_ctx 1024 -n_batch 2 -p "Translate the following English sentence to Japanese:" -n_predict 128
```

**Common flags**

- `-p` – prompt text.
- `-n_predict` – number of tokens to generate.
- `-stream` – stream output token‑by‑token.

---

### 7. Monitor VRAM Usage

Use `nvidia-smi` to check real‑time memory usage:

```bash
watch -n 0.5 nvidia-smi
```

You should see a peak around ~4–5 GB (with the settings above).
If you hit >6 GB, reduce `-ngl`, `-n_ctx`, or switch to a smaller model.

---

## Troubleshooting Checklist

| Symptom                        | Likely Cause                                | Fix                                                              |
| ------------------------------ | ------------------------------------------- | ---------------------------------------------------------------- |
| **Segmentation fault / crash** | GPU memory overflow                         | Reduce `-ngl` or switch to a smaller model.                      |
| **Very slow generation**       | CPU threads too low or batch size too small | Increase `-t` (but watch CPU load) and `-n_batch`.               |
| **Excessive CPU usage**        | GPU not being used                          | Make sure you compiled with `CUBLAS=1` and that CUDA is present. |
| **Out‑of‑memory on GPU**       | Model too big for 6 GB                      | Use a quantized model (4‑bit) or a smaller variant (1.3B).       |

---

## Summary – What to Adjust & In What Order

1. **Compile with CUDA** (`CUBLAS=1`).
2. **Quantize the model to 4‑bit** (or 8‑bit if you have more VRAM).
3. **Set `-ngl`** to a value that comfortably fits your GPU (e.g., 24 or higher).
4. **Limit `-n_ctx`** to 1024 if you want extra safety.
5. **Choose `-t`** (CPU threads) and `-n_batch` (batch size) for a balance of speed vs. memory.
6. **Run** and monitor with `nvidia-smi`.
7. **Iterate**: If you hit memory limits, reduce `-ngl` or `-n_ctx`. If you’re under‑utilizing the GPU, increase `-ngl` or use a larger model.

Follow the checklist above, and you’ll have GPT‑OSS running smoothly on your RTX 2060 in no time. Happy modeling!

</details>

---

## Tuning llama.cpp

### Quantize a model

Running `llama-cli -hf ggml-org/gpt-oss-20b-GGUF` downloads the model `ggml-org_gpt-oss-20b-GGUF_gpt-oss-20b-mxfp4.gguf`. RTX 2060 doesn't support fp4, so try to quantize myself.

1.  `sudo apt install git-lfs`
2.  `mkdir -p ~/huggingface.co/openai && cd mkdir ~/huggingface.co/openai`
3.  `git clone https://huggingface.co/openai/gpt-oss-20b && cd gpt-oss-20b`
4.  To create venv before installing python modules, ran `sudo apt install -y python3-venv && python3 -m venv venv && source venv/bin/activate`
5.  `pip install -r ~/github/ggml-org/llama.cpp/requirements.txt`
6.  `mkdir out`
7.  To convert a model, ran `python3 ~/github/ggml-org/llama.cpp/convert_hf_to_gguf.py --model_dir . --outfile gpt-oss-20b-f16.gguf --outtype f16 .`
8.  To quantize, ran `~/github/ggml-org/llama.cpp/build/bin/llama-quantize --allow-requantize out/gpt-oss-20b-f16.gguf out/gpt-oss-20b-f16-Q4_K_M.gguf Q4_K_M` .

Unfortunately, Q4_K_M makes the model size bigger.
| size | model |
| ----------- | ----------- |
| 13369094144 | Q4_1 |
| 15805136384 | Q4_K_M |
| 13792638464 | f16 |
| 11618123200 | Intel/q4ks-AutoRound |

There are many conversion fallbacks. Fallback mxfp4 to other causes size inflation. According to the quantizer's message, Q1_K requires the block size to be a multiple of 256. This is likely because of the size of the matrix columns is 2,880.

Type Q4_1 didn't cause such a fallbacks, but the AI generated corrupt responses. This suggests that the weight precision was lost too much.

I gave up on the investigation and I decided to use the Intel's model.

---

### Find good parameters

According to [guide : running gpt-oss with llama.cpp](https://github.com/ggml-org/llama.cpp/discussions/15396), `--n-cpu-moe` seems to increase the token generation speed. In my case, for i5 14550, --n-cpu-moe 17 might not be a bad number. 14550 has 14 physical cores and 20 logical threads. It seems that 8 might be too small, but 20 might be too big.

We can use the option `--threads`. If the number is too small or too big, the speed of token generation will be slower. If we don't specify it, llama.cpp might use a better number of threads. Therefore, we should not specify this option.

There is the option `-fa` that specifies whether the flash attention is enabled. This does not appear to affect the speed of token generation.

In my case, the batch size option `-b 1` decreases the speed. A value greater than 1 does not affect to the speed. I think the batch count might be ignored.

---

After conducting an investigation, I compared the token generation performance of OpenAI's original model with that of Intel's AutoRound model. There was no significant difference. Note that I did not compare quality.

<!-- cSpell:words ixpkgs addrepo nixpkgs persistenced libcuda openai mxfp4 gptuser libggml dtype requantize -->
<!-- cSpell:words DGGML_CUDA DCMAKE_CUDA_ARCHITECTURES DCMAKE_CUDA_COMPILER DLLAMA_CURL DCMAKE_BUILD_TYPE -->
<!-- general tech terms cSpell:words toolbx ggml gguf jinja xorg xconfig nvidia cuda nvcc rsqrt linux noexcept devel ccache venv CUBLAS ONNX BLAS quantizer -->
